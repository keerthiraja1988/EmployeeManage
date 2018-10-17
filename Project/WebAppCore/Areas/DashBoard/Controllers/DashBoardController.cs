using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CrossCutting.Logging;
using DomainModel.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _IHttpContextAccessor;

        public DashBoardController(
                 IDashBoardService IDashBoardService
                   , IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _IHttpContextAccessor = httpContextAccessor;

            _iDashBoardService = IDashBoardService;
        }

        [Route("Home")]
        public IActionResult Index()
        {
            var userIpAddress = this._IHttpContextAccessor
                                                .HttpContext.Connection.RemoteIpAddress
                                                .ToString();
            if (userIpAddress == "::1")
            {
                userIpAddress = "35.225.15.176";
            }
            DashBoardRow1WidgetsModel dashBoardRow1WidgetsModel = new DashBoardRow1WidgetsModel();
            WeatherWidgetsViewModel weatherWidgetsViewModel = new WeatherWidgetsViewModel();
            WeatherModel weatherModel = new WeatherModel();

            DashBoardWidgetsDTO dashBoardWidgetsDTO = new DashBoardWidgetsDTO();
            dashBoardWidgetsDTO.DashBoardRow1WidgetsViewModel = new DashBoardRow1WidgetsViewModel();

            var dashBoardRow1WidgetsModelTask = Task.Run(() =>
                        this._iDashBoardService.GetBoardRow1WidgetsDetails()
                    );
            var dashBoardWeatherWidgetTask = Task.Run(() =>
            this._iDashBoardService.GetCurrectWeatherDetails(userIpAddress)
        );
            Task.WhenAll(dashBoardRow1WidgetsModelTask, dashBoardWeatherWidgetTask);

            dashBoardRow1WidgetsModel = dashBoardRow1WidgetsModelTask.Result;
            dashBoardWidgetsDTO.DashBoardRow1WidgetsViewModel = _mapper.Map<DashBoardRow1WidgetsViewModel>(dashBoardRow1WidgetsModel);

            weatherModel = dashBoardWeatherWidgetTask.Result;
            dashBoardWidgetsDTO.DashBoardRow2WidgetsViewModel = new DashBoardRow2WidgetsViewModel();
            dashBoardWidgetsDTO.DashBoardRow2WidgetsViewModel.WeatherWidgetsViewModel =
                        _mapper.Map<WeatherWidgetsViewModel>(weatherModel);

            return View("Index", dashBoardWidgetsDTO);
        }
    }
}