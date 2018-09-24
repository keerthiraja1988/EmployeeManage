using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAppCore.Controllers
{
    [Area("Base")]
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ValidatePassword(string Password)
        {
            return Json("Test");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> Foo(string name)
        {
            //bool exists = false;
            //if (exists)
            //    return Json(data: false);
            //else
                return Json(data: true);
        }

    }
}