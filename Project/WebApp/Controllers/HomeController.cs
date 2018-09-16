using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public HomeController(IConfiguration iConfig)
        {
            Configuration = iConfig;
        }

        public IActionResult Index()
        {
            var str = Configuration.GetValue<string>("ApplicationsSetting:SQLConnection");
            ViewBag.AppSQLConfig = Configuration.GetValue<string>("ApplicationsSetting:SQLConnection");

            return View("Index");
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
