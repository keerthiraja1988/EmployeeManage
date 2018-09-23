using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DomainModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceInterface;
using WebAppCore.Areas.Security.Models;
using WebAppCore.Infrastructure;
using WebAppCore.Models;

namespace WebAppCore.Areas.Security.Controllers
{
    [Area("Security")]
    public class UserAccountController : Controller
    {
        public IUserAccountService _IUserAccountService { get; set; }
        public IConfiguration Configuration { get; set; }

        private readonly IMapper _mapper;
        public UserAccountController(IConfiguration iConfig, IUserAccountService iUserAccountService
            , IMapper mapper)
        {
            _mapper = mapper;
            Configuration = iConfig;
            _IUserAccountService = iUserAccountService;
        }


        //public ActionResult Index()
        //{
        //    RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();
        //    UserLoginViewModel userLoginViewModel = new UserLoginViewModel();

        //    UserLoginRegisterDTO userLoginRegisterDTO = new UserLoginRegisterDTO();
        //    userLoginRegisterDTO.RegisterUserViewModel = registerUserViewModel;
        //    userLoginRegisterDTO.UserLoginViewModel = userLoginViewModel;
        //    //  await Task.Run(() => ));
        //    //  return await Task.Run(() => View(userLoginRegisterDTO));
        //    return View(userLoginRegisterDTO);
        //}

        [Route("UserAccount")]
        [HttpGet]
        public async Task<IActionResult> UserAccount()
        {
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

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Sean Connery"),
                new Claim(ClaimTypes.Email, userLoginViewModel.LoginUserName)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // create principal
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Json(new { newUrl = Url.Action("Index", "DashBoard", new { area = "DashBoard" }) });

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