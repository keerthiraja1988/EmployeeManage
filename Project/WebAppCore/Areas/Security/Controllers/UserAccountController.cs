using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CrossCutting.Logging;
using DomainModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    //[HandleException]
    [NLogging]
    public class UserAccountController : Controller
    {
        public IUserAccountService _IUserAccountService { get; set; }
        public IConfiguration _Configuration { get; set; }

        private readonly IMapper _mapper;
        public UserAccountController(IConfiguration iConfig, IUserAccountService iUserAccountService
            , IMapper mapper)
        {
            _mapper = mapper;
            _Configuration = iConfig;
            _IUserAccountService = iUserAccountService;
        }

        [Route("UserAccount")]
        [HttpGet]
      
        public async Task<IActionResult> UserAccount(string redirectUrl)
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
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel, string ReturnUrl)
        {

            var cookieAvailable = CookieAuthenticationDefaults.AuthenticationScheme;

            if (cookieAvailable != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            }
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => PartialView("_Login", userLoginViewModel));
            }
            var userAccount = _mapper.Map<UserAccountModel>(userLoginViewModel);
            var userAccountReturn = this._IUserAccountService.ValidateUserLogin(userAccount);
            if (userAccountReturn.IsLoginSuccess)
            {
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccountReturn.LastName + " " + userAccountReturn.FirstName),
                new Claim(ClaimTypes.Email, userAccountReturn.Email)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // create principal
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Json(new { newUrl = Url.Action("Index", "DashBoard", new { area = "DashBoard" }) });
            }
            else
            {
                return Json(new { newUrl = "LoginFailed" });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public async Task<IActionResult> AutoPopulateRegsitration()
        {
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();

            var UserAccountModel = this._IUserAccountService.GetAutoGenetaratedUserData();

            registerUserViewModel = _mapper.Map<RegisterUserViewModel>(UserAccountModel);
            registerUserViewModel.ReTypePassword = registerUserViewModel.Password;

            string partialViewHtml = await this.RenderViewAsync("_Register", registerUserViewModel, true);

            return Json(partialViewHtml);
        }

        public async Task<IActionResult> Logout()
        {
            var cookieAvailable = CookieAuthenticationDefaults.AuthenticationScheme;
            if (cookieAvailable != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return Redirect("/UserAccount");
        }

        
    }
}