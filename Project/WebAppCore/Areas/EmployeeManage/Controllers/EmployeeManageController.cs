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
using WebAppCore.Infrastructure.Filters;
using WebAppCore.Infrastructure;
using DomainModel.EmployeeManage;

namespace WebAppCore.Areas.EmployeeManage.Controllers
{
    [Area("EmployeeManage")]
    [NLogging]
    [Roles("SuperUser", "Admin")]
    public class EmployeeManageController : Controller
    {
        public IEmployeeManageService _IEmployeeManageService { get; set; }
        public IConfiguration _configuration { get; set; }

        private readonly IMapper _mapper;
        public EmployeeManageController(IConfiguration iConfig, IEmployeeManageService iEmployeeManageService
            , IMapper mapper)
        {
            _mapper = mapper;
            _configuration = iConfig;
            _IEmployeeManageService = iEmployeeManageService;
        }

        public async Task<IActionResult> Index()
        {
           // var getUserDetailsTask1 = this._IEmployeeManageService.LoadEmployeeData();
            var getUserDetailsTask = Task.Run(() => this.User.GetLoggedInUserDetails());
            var loggedInUserDetails = await getUserDetailsTask;
            return View();
        }

        [Route("GetEmployeeDetails")]
        public async Task<IActionResult> Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            var employeeDetails = await this._IEmployeeManageService.GetEmployeesDetails();
            List<EmployeeViewModel> EmployeesViewModel = new List<EmployeeViewModel>();

            EmployeesViewModel = _mapper.Map<List<EmployeeViewModel>>(employeeDetails);

            return Json(EmployeesViewModel.ToDataSourceResult(request));
        }


        [Route("GetEmployeeDetailsForSearch")]
        public async Task<IEnumerable<EmployeeSearchViewModel>> GetEmployeeDetailsForSearch(EmployeeSearchViewModel employeeSearchViewModel)
        {
            IEnumerable<EmployeeSearchViewModel> employeeSearchResult = new List<EmployeeSearchViewModel>();
            EmployeeSearchModel employeeSearchModel = new EmployeeSearchModel();
            employeeSearchModel = _mapper.Map<EmployeeSearchModel>(employeeSearchViewModel);

            var employeSearchDetails = await this._IEmployeeManageService
                                            .GetEmployeesDetailsForSearch(employeeSearchModel);

            employeeSearchResult = _mapper.Map<List<EmployeeSearchViewModel>>(employeSearchDetails);

            return employeeSearchResult;
        }

        [Route("GetEmployeeDetailsForSearch1")]
        public IActionResult ServerFiltering(string text, string TT)
        {
            return View();
        }

    }
}