using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.Areas.Security.Models;
using WebAppCore.Models;

namespace WebAppCore.Areas.Security.Controllers
{
    [Area("Security")]
    public class UserAccountController : Controller
    {

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

            return await Task.Run(() => View("Login", new UserLoginRegisterDTO()));

        }

        //[Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            UserLoginRegisterDTO userLoginRegisterDTO = new UserLoginRegisterDTO();
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();
            userLoginRegisterDTO.RegisterUserViewModel = registerUserViewModel;
            userLoginRegisterDTO.UserLoginViewModel = userLoginViewModel;

            if (!ModelState.IsValid)
            {
                return await Task.Run(() => View("Login", userLoginRegisterDTO));


            }
            return await Task.Run(() => View("Login", userLoginRegisterDTO));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            UserLoginRegisterDTO userLoginRegisterDTO = new UserLoginRegisterDTO();
            UserLoginViewModel userLoginViewModel = new UserLoginViewModel();

            userLoginRegisterDTO.RegisterUserViewModel = registerUserViewModel;
            userLoginRegisterDTO.UserLoginViewModel = userLoginViewModel;
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => View("Login", userLoginRegisterDTO));

            }


            return await Task.Run(() => View("Login", userLoginRegisterDTO));

        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}