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
using System.Globalization;

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
        public async Task<IActionResult> Products_Read([DataSourceRequest] DataSourceRequest request
                                                                , EmployeeSearchViewModel employeeSearchViewModel)
        {
            EmployeeSearchModel employeeSearchModel = new EmployeeSearchModel();
            employeeSearchModel = _mapper.Map<EmployeeSearchModel>(employeeSearchViewModel);

            var employeeDetails = await this._IEmployeeManageService.GetEmployeesDetailsOnSearch(employeeSearchModel);
            List<EmployeeViewModel> EmployeesViewModel = new List<EmployeeViewModel>();

            EmployeesViewModel = _mapper.Map<List<EmployeeViewModel>>(employeeDetails);

            return Json(EmployeesViewModel.ToDataSourceResult(request));
        }

        [HttpPost]
        [Route("GetEmployeeDetailsForEdit")]
        public async Task<IActionResult> GetEmployeeDetailsForEdit([FromBody] EmployeeViewModel employeeSearchViewModel)
        {
            EmployeeModel employeeSearchModel = new EmployeeModel();
            employeeSearchModel = _mapper.Map<EmployeeModel>(employeeSearchViewModel);

            var employeeDetails = await this._IEmployeeManageService.GetEmployeeDetails(employeeSearchModel);

            EmployeeViewModel EmployeesViewModel = new EmployeeViewModel();

            EmployeesViewModel = _mapper.Map<EmployeeViewModel>(employeeDetails);

            string partialViewHtml = await this.RenderViewAsync("_EditEmployeeDetails", EmployeesViewModel, true);

            return Json(partialViewHtml);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EditEmployeeDetails")]
        public async Task<IActionResult> EditEmployeeDetails(EmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => PartialView("_EditEmployeeDetails", employeeViewModel));
            }

            int isDBCallSucces = await SaveEmployeeDetails(employeeViewModel);

            return Json("");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromBody] EmployeeViewModel employeeViewModel)
        {
            EmployeeModel employeeModel = new EmployeeModel();
            employeeModel = _mapper.Map<EmployeeModel>(employeeViewModel);

            int isDBCallSucces = await this._IEmployeeManageService.DeleteEmployee(employeeModel);

            if (isDBCallSucces == 0)
            {
              return  new JsonResult("RequestPassed|0|Success|Employee Deleted Successfully");
            }
            else
            {
                return new JsonResult("RequestFailed|1|Error|Error Occurred on Employee Deletion");
            }
           
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
        [HttpPost]
        [Route("ValidateEmployeeDetailsOnSearch")]
        public async Task<IActionResult> ValidateEmployeeDetailsOnSearch([FromBody] EmployeeSearchViewModel employeeSearchViewModel
                            )
        {
            var TaskValidateSearchAllFiled = ValidateEmployeeDetailsOnSearchAllFiled(employeeSearchViewModel);
            var TaskValidateSearchDOB = ValidateEmployeeDetailsOnDOB(employeeSearchViewModel);
            var TaskValidateSearchDOJ = ValidateEmployeeDetailsOnDOJ(employeeSearchViewModel);

            var ValidateResults = await Task.WhenAll(TaskValidateSearchAllFiled
                                                                , TaskValidateSearchDOB
                                                                ,TaskValidateSearchDOJ);

            foreach (var validateResults in ValidateResults)
            {
                if (validateResults.Length > 0)
                {
                    return await Task.Run(() =>
                   new JsonResult(validateResults));
                }
            }

            return await Task.Run(() =>
                    new JsonResult("RequestPassed|0|Success|Validate Employee Details On Search Success"));
        }

        #region Private Methods EmployeeManage Controller

        private async Task<int> SaveEmployeeDetails(EmployeeViewModel employeeViewModel)
        {
            employeeViewModel.DateOfBirth = DateTime.Now;
            employeeViewModel.DateOfJoining = DateTime.Now;

            EmployeeModel employeeModel = new EmployeeModel();
            List<EmployeeAddressModel> employeeAddresses = new List<EmployeeAddressModel>();
            employeeModel = _mapper.Map<EmployeeModel>(employeeViewModel);

            return await this._IEmployeeManageService.EditEmployeeDetails(employeeModel
                                                                                , employeeAddresses);
        }

        private static async Task<string> ValidateEmployeeDetailsOnSearchAllFiled(EmployeeSearchViewModel employeeSearchViewModel)
        {
            if (employeeSearchViewModel == null || (employeeSearchViewModel.DateOfBirthEnd == null
                && employeeSearchViewModel.DateOfBirthStart == null && employeeSearchViewModel.DateOfJoiningEnd == null
                && employeeSearchViewModel.DateOfJoiningStart == null && employeeSearchViewModel.Email == ""
                && employeeSearchViewModel.EmployeeId == "" && employeeSearchViewModel.FullName == ""
                && employeeSearchViewModel.Passport == "" && employeeSearchViewModel.TIN == ""
                ))
            {
                return "RequestFailed|2|Warning|At least provide one criteria for search";
            }

            return "";
        }

        private static async Task<string> ValidateEmployeeDetailsOnDOJ(EmployeeSearchViewModel employeeSearchViewModel)
        {

            if (employeeSearchViewModel != null && (employeeSearchViewModel.DateOfJoiningStart != null && employeeSearchViewModel.DateOfJoiningEnd == null))
            {
                return "RequestFailed|2|Warning|Please provide Date Of Joining End date ";
            }
            else if (employeeSearchViewModel != null && (employeeSearchViewModel.DateOfJoiningStart == null && employeeSearchViewModel.DateOfJoiningEnd != null))
            {
                return "RequestFailed|2|Warning|Please provide Date Of Joining Start date ";
            }
            else if (employeeSearchViewModel != null && (employeeSearchViewModel.DateOfJoiningStart != null && employeeSearchViewModel.DateOfJoiningEnd != null)
                && (employeeSearchViewModel.DateOfJoiningStart > employeeSearchViewModel.DateOfJoiningEnd))
            {
                return "RequestFailed|2|Warning| Date Of Joining Start date should be less than End Date ";
            }

            return "";
        }

        private static async Task<string> ValidateEmployeeDetailsOnDOB(EmployeeSearchViewModel employeeSearchViewModel)
        {

            if (employeeSearchViewModel != null && (employeeSearchViewModel.DateOfBirthStart != null && employeeSearchViewModel.DateOfBirthEnd == null))
            {
                return "RequestFailed|2|Warning|Please provide Date Of Birth End date ";
            }
            else if (employeeSearchViewModel != null && (employeeSearchViewModel.DateOfBirthStart == null && employeeSearchViewModel.DateOfBirthEnd != null))
            {
                return "RequestFailed|2|Warning|Please provide Date Of Birth Start date ";
            }
            else if (employeeSearchViewModel != null && (employeeSearchViewModel.DateOfBirthStart != null && employeeSearchViewModel.DateOfBirthEnd != null)
                && (employeeSearchViewModel.DateOfBirthStart > employeeSearchViewModel.DateOfBirthEnd))
            {
                return "RequestFailed|2|Warning| Date Of Birth Start date should be less than End Date ";
            }

            return "";
        }

        #endregion
    }
}