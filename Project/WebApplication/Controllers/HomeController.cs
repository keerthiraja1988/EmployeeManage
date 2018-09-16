using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
          

            return View();
        }

        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<UserAccountViewModel> UserAccounts = new List<UserAccountViewModel>();
            UserAccounts = Builder<UserAccountViewModel>.CreateListOfSize(600)
                                      .All()
                               .With(c => c.UserName = Faker.Name.FullName())
                                  .With(c => c.Email = Faker.User.Email())
                           .Build().ToList();

            return Json(UserAccounts.ToDataSourceResult(request));
        }
    }
}