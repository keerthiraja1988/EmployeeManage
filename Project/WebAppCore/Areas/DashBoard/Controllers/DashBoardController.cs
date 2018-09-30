using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting.Logging;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.Areas.DashBoard.Models;
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

            DashBoardWidgetsDTO dashBoardWidgetsDTO = new DashBoardWidgetsDTO();
            dashBoardWidgetsDTO.DashBoardRow1Widgets = new DashBoardRow1Widgets();
            dashBoardWidgetsDTO.DashBoardRow1Widgets.TotalNoOfEmployees = "5210";
            dashBoardWidgetsDTO.DashBoardRow1Widgets.NoOfEmployeesCreatedToday = "25";
            dashBoardWidgetsDTO.DashBoardRow1Widgets.NoOfEmployeesPendingAuth= "37";

            return View("Index", dashBoardWidgetsDTO);
        }
    }
}