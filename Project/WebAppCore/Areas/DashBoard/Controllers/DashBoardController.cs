using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.Infrastructure.Filters;

namespace WebAppCore.Areas.DashBoard.Controllers
{
    [Roles("SuperUser")]
    [Area("DashBoard")]
    [NLogging]
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