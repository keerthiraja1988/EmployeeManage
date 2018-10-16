using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CrossCutting.Logging;
using DomainModel;
using DomainModel.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceInterface;
using WebAppCore.Areas.Security.Models;
using WebAppCore.Infrastructure;
using WebAppCore.Infrastructure.Filters;
using WebAppCore.Models;

namespace WebAppCore.Areas.Security.Controllers
{
    [Area("Security")]
    [NLogging]
    public class UserAccountController : Controller
    {
        public IUserAccountService _IUserAccountService { get; set; }
        public IConfiguration _Configuration { get; set; }
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IMapper _mapper;
        IAppAnalyticsService _IAppAnalyticsService;
        public UserAccountController(IConfiguration iConfig, IUserAccountService iUserAccountService
            , IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IAppAnalyticsService iAppAnalyticsService)
        {
            _mapper = mapper;
            _Configuration = iConfig;
            _IUserAccountService = iUserAccountService;
            _IHttpContextAccessor = httpContextAccessor;
            _IAppAnalyticsService = iAppAnalyticsService;
        }

        [Route("UserAccount")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserAccount(string returnUrl = null)
        {
            //throw new Exception();
            var cookieAvailable = CookieAuthenticationDefaults.AuthenticationScheme;

            if (cookieAvailable != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            UserLoginRegisterDTO userLoginRegisterDTO = new UserLoginRegisterDTO();

            return await Task.Run(() => View("Login", userLoginRegisterDTO));
        }

        //[Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel, string ReturnUrl)
        {
            userLoginViewModel.UserIpAddress = this._IHttpContextAccessor
                                                .HttpContext.Connection.RemoteIpAddress
                                                .ToString();

            var cookieAvailable = CookieAuthenticationDefaults.AuthenticationScheme;

            if (cookieAvailable != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => PartialView("_Login", userLoginViewModel));
            }

            UserAccountModel userAccountReturn;
            List<UserRolesModel> userRoles;

            ValidateLoginUserDetails(userLoginViewModel, out userAccountReturn, out userRoles);

            if (userAccountReturn.IsLoginSuccess)
            {
                return await SetCookiesAndReturnViewOnLoginSuccess(userAccountReturn, userRoles);
            }
            else
            {
                return Json(new { newUrl = "LoginFailed" });
            }
        }

        private void ValidateLoginUserDetails(UserLoginViewModel userLoginViewModel, out UserAccountModel userAccountReturn, out List<UserRolesModel> userRoles)
        {
            var userAccount = _mapper.Map<UserAccountModel>(userLoginViewModel);
            var validateUserLoginResult = this._IUserAccountService.ValidateUserLogin(userAccount);

            userAccountReturn = new UserAccountModel();
            userRoles = new List<UserRolesModel>();
            userAccountReturn = validateUserLoginResult.UserAccount;
            userRoles = validateUserLoginResult.UserRoles;
        }

        private async Task<IActionResult> SetCookiesAndReturnViewOnLoginSuccess(UserAccountModel userAccountReturn, List<UserRolesModel> userRoles)
        {
            List<Claim> claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, userAccountReturn.LastName + " " + userAccountReturn.FirstName),
                new Claim ( "http://example.org/claims/UserName", "UserName", userAccountReturn.UserName),
                new Claim ( "http://example.org/claims/FirstName", "FirstName", userAccountReturn.FirstName),
                new Claim ( "http://example.org/claims/LastName", "LastName", userAccountReturn.LastName),

                    new Claim(ClaimTypes.NameIdentifier , userAccountReturn.UserId.ToString()),
                new Claim("http://example.org/claims/LoggedInTime", "LoggedInTime", DateTime.Now.ToString()),
                new Claim(ClaimTypes.Email, userAccountReturn.Email),
                new Claim ( "http://example.org/claims/CookieUniqueId", "CookieUniqueId",userAccountReturn.CookieUniqueId.ToString() ),

            };

            if (userRoles != null && userRoles.Count > 0)
            {
                foreach (var item in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item.RoleName));
                }
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // create principal
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Json(new { newUrl = Url.Action("Index", "DashBoard", new { area = "DashBoard" }) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => PartialView("_Register", registerUserViewModel));
            }

            var userAccount = _mapper.Map<UserAccountModel>(registerUserViewModel);
            var returnValue = this._IUserAccountService.RegisterNewUser(userAccount);

            string partialViewHtml = await this.RenderViewAsync("_RegistrationSuccess", registerUserViewModel, true);

            return Json(partialViewHtml);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AutoPopulateRegsitration()
        {
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();

            var UserAccountModel = this._IUserAccountService.GetAutoGeneratedUserData();

            registerUserViewModel = _mapper.Map<RegisterUserViewModel>(UserAccountModel);
            registerUserViewModel.ReTypePassword = registerUserViewModel.Password;

            string partialViewHtml = await this.RenderViewAsync("_Register", registerUserViewModel, true);

            return Json(partialViewHtml);
        }

        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            var cookieAvailable = CookieAuthenticationDefaults.AuthenticationScheme;
            if (cookieAvailable != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return Redirect("/UserAccount");
        }

        [Route("AccessDenied")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied()
        {
            return await Task.Run(() => View("AccessDenied"));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetLoggedInUserDetails()
        {
            LoggedInUserDetailsViewModel loggedInUserDetailsViewModel = await GetUserDetailsFromCookies();
            UserAccountModel userAccountModel = _mapper.Map<UserAccountModel>(loggedInUserDetailsViewModel);
            var userLoginDetails = this._IUserAccountService.GetUserDetailsForLastLogin(userAccountModel);

            loggedInUserDetailsViewModel.LastLoggedInUserDetailsViewModel = new LoggedInUserDetailsViewModel();
            loggedInUserDetailsViewModel.CurrentLoggedInUserDetailsViewModel = new LoggedInUserDetailsViewModel();

            IpPropertiesModal ipPropertiesModal = new IpPropertiesModal();
            string ipAddress = this._IHttpContextAccessor
                                    .HttpContext.Connection.RemoteIpAddress
                                    .ToString();
            ipPropertiesModal = this._IAppAnalyticsService.GetIpAddressDetails(ipAddress);

            loggedInUserDetailsViewModel.LastLoggedInUserDetailsViewModel =
                    _mapper.Map<LoggedInUserDetailsViewModel>(userLoginDetails.LastSessionDetails);
            
            loggedInUserDetailsViewModel.CurrentLoggedInUserDetailsViewModel =
                    _mapper.Map<LoggedInUserDetailsViewModel>(userLoginDetails.CurrentSessionDetails);

            string partialViewHtml = await this.RenderViewAsync("_LoggedInUserDetails", loggedInUserDetailsViewModel, true);

            return Json(partialViewHtml);

        }

        private async Task<LoggedInUserDetailsViewModel> GetUserDetailsFromCookies()
        {
            var getUserDetailsTask = Task.Run(() => this.User.GetLoggedInUserDetails());
            var loggedInUserDetails = await getUserDetailsTask;
            LoggedInUserDetailsViewModel loggedInUserDetailsViewModel = new LoggedInUserDetailsViewModel();
            loggedInUserDetailsViewModel.UserName = loggedInUserDetails.UserName;
            loggedInUserDetailsViewModel.FirstName = loggedInUserDetails.FirstName;
            loggedInUserDetailsViewModel.LastName = loggedInUserDetails.LastName;
            loggedInUserDetailsViewModel.UserId = loggedInUserDetails.UserId;
            loggedInUserDetailsViewModel.UserRoles = loggedInUserDetails.UserRoles;
            loggedInUserDetailsViewModel.Email = loggedInUserDetails.Email;
            loggedInUserDetailsViewModel.CookieUniqueId = loggedInUserDetails.CookieUniqueId;

            return loggedInUserDetailsViewModel;
        }
    }
}