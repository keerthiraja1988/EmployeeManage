using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppCore.Areas.DashBoard.Controllers
{
    [Authorize]
    [Area("DashBoard")]
    public class DashBoardController : Controller
    {
        [Route("")]
        [Route("Home")]
        [NlogTrace]
        public IActionResult Index()
        {
            //throw new Exception();

           return View("Index");
        }
    }
}