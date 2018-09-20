using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.UserAccount.Models;

namespace WebApp.Areas.UserAccount.Controllers
{
    [Area("Security")]
    [Route("UserAccount")]
    public class UserAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel inputModel)
        {
            //if (!IsAuthentic(inputModel.Username, inputModel.Password))
            //    return View();

            // create claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Sean Connery"),
                new Claim(ClaimTypes.Email, inputModel.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // create principal
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

    }
}