using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceInterface;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebAppCore.Areas.EmployeeManage.Models;
using CrossCutting.Logging;

namespace WebAppCore.Areas.EmployeeManage.Controllers
{
    [Authorize]
    [Area("EmployeeManage")]
   [NLogging]
    public class EmployeeManageController : Controller
    {
        public IEmployeeManageService _IEmployeeManageService { get; set; }
        public IConfiguration Configuration { get; set; }

        private readonly IMapper _mapper;
        public EmployeeManageController(IConfiguration iConfig, IEmployeeManageService iEmployeeManageService
            , IMapper mapper)
        {
            _mapper = mapper;
            Configuration = iConfig;
            _IEmployeeManageService = iEmployeeManageService;
        }


        public IActionResult Index()
        {

            //var vvv = this._IEmployeeManageService.LoadEmployeeData();

            return View();
        }
        [Route("GetEmployeeDetails")]
        public async Task<IActionResult> Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            var employeeDetails = await this._IEmployeeManageService.GetEmployeesDetails();
            List<EmployeeViewModel> EmployeesViewModel = new List<EmployeeViewModel>();

            EmployeesViewModel =  _mapper.Map<List<EmployeeViewModel>>(employeeDetails);
          //  throw new Exception();
            return Json(EmployeesViewModel.ToDataSourceResult(request));
        }
    }
}