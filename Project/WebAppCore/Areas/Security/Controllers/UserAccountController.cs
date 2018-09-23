using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DomainModel;
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
            UserLoginRegisterDTO userLoginRegisterDTO = new UserLoginRegisterDTO();

            //RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();
            //UserLoginViewModel userLoginViewModel = new UserLoginViewModel();
            //registerUserViewModel.UserName = "test";
            //userLoginRegisterDTO.RegisterUserViewModel = registerUserViewModel;
            //userLoginRegisterDTO.UserLoginViewModel = userLoginViewModel;
            return await Task.Run(() => View("Login", userLoginRegisterDTO));

        }

        //[Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => PartialView("_Login", userLoginViewModel));
            }
            return await Task.Run(() => PartialView("_Login", userLoginViewModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            if (registerUserViewModel.UserName == "" || registerUserViewModel.UserName == null)
            {
                registerUserViewModel.UserName = "test";
            }
           
            UserLoginRegisterDTO userLoginRegisterDTO = new UserLoginRegisterDTO();
            UserLoginViewModel userLoginViewModel = new UserLoginViewModel();

            userLoginRegisterDTO.RegisterUserViewModel = registerUserViewModel;
            userLoginRegisterDTO.UserLoginViewModel = userLoginViewModel;
            if (!ModelState.IsValid)
            {
                //  return await Task.Run(() => View("Login", userLoginRegisterDTO));
                return await Task.Run(() => PartialView("_Register", registerUserViewModel));
            }

            var userAccount = _mapper.Map<UserAccountModel>(registerUserViewModel);
            var returnValue = this._IUserAccountService.RegisterNewUser(userAccount);
            var returnValue1 = this._IUserAccountService.RegisterNewUser(userAccount);

            return await Task.Run(() => PartialView("_Register", registerUserViewModel));

        }

       
        public async Task<IActionResult> AutoPopulateRegsitration()
        {
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();
            UserLoginViewModel userLoginViewModel = new UserLoginViewModel();
            registerUserViewModel.UserName = "test";
            UserLoginRegisterDTO userLoginRegisterDTO = new UserLoginRegisterDTO();
            userLoginRegisterDTO.RegisterUserViewModel = registerUserViewModel;
            userLoginRegisterDTO.UserLoginViewModel = userLoginViewModel;
            //  await Task.Run(() => ));

          string  partialViewHtml = await this.RenderViewAsync("_Register", registerUserViewModel, true);

            // return await Task.Run(() => PartialView("_Register", registerUserViewModel));
            //return Content(partialViewHtml);

           return Json(partialViewHtml);
        }

    }
}