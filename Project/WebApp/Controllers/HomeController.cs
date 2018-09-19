using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting.Logging;
using FizzWare.NBuilder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceInterface;
using WebApp.Infrastructure;
using WebApp.Models;

namespace WebApp.Controllers
{

    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; set; }
        public IUserAccountService _IUserAccountService { get; set; }

        
        public HomeController(IConfiguration iConfig, IUserAccountService iUserAccountService)
        {
            Configuration = iConfig;
            _IUserAccountService = iUserAccountService;
        }

        [NlogTrace]
        public IActionResult Index()
        {

            var testValue = this._IUserAccountService.GetTestValue();
            var str = Configuration.GetValue<string>("ApplicationsSetting:SQLConnection");
            ViewBag.AppSQLConfig = Configuration.GetValue<string>("ApplicationsSetting:SQLConnection")
                                    + "  Test Value = " + testValue.ToString()
                                    ;

            //throw new Exception();
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
