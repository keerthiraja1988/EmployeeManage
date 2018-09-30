using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CrossCutting.Logging;
using DomainModel.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceInterface;
using WebAppCore.Areas.DashBoard.Models;
using WebAppCore.Infrastructure.Filters;

namespace WebAppCore.Areas.DashBoard.Controllers
{
    [Roles("SuperUser")]
    [Area("DashBoard")]
    [NLogging]
    public class DashBoardController : Controller
    {
        private IDashBoardService _iDashBoardService;
        private readonly IMapper _mapper;

        public DashBoardController(
                 IDashBoardService IDashBoardService
                   , IMapper mapper)
        {
            _mapper = mapper;
            
            _iDashBoardService = IDashBoardService;
        }
        [Route("")]
        [Route("Home")]
        public IActionResult Index()
        {
            DashBoardRow1WidgetsModel dashBoardRow1WidgetsModel = new DashBoardRow1WidgetsModel();
            var dashBoardRow1WidgetsModelTask = Task.Run(() =>
                                    this._iDashBoardService.GetBoardRow1WidgetsDetails()
                                );
            DashBoardWidgetsDTO dashBoardWidgetsDTO = new DashBoardWidgetsDTO();
            dashBoardWidgetsDTO.DashBoardRow1WidgetsViewModel = new DashBoardRow1WidgetsViewModel();

            dashBoardRow1WidgetsModelTask.Wait();
            dashBoardRow1WidgetsModel = dashBoardRow1WidgetsModelTask.Result;
            dashBoardWidgetsDTO.DashBoardRow1WidgetsViewModel = _mapper.Map<DashBoardRow1WidgetsViewModel>(dashBoardRow1WidgetsModel);

            return View("Index", dashBoardWidgetsDTO);
        }
    }
}