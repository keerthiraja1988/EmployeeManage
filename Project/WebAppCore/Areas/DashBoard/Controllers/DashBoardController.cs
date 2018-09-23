using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}