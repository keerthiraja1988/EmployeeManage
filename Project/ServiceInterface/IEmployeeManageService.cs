using DomainModel.EmployeeManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IEmployeeManageService
    {
        Task<List<EmployeeModel>> LoadEmployeeData();

        Task<List<EmployeeModel>> GetEmployeesDetails();

        Task<EmployeeModel> GetEmployeeDetails(EmployeeModel employeeSearchModel);

        Task<List<EmployeeSearchModel>> GetEmployeesDetailsForSearch(EmployeeSearchModel employeeSearchModel);

        Task<List<EmployeeModel>> GetEmployeesDetailsOnSearch(EmployeeSearchModel employeeSearchModel);

        Task<int> EditEmployeeDetails(EmployeeModel employeeModel, List<EmployeeAddressModel> employeeAddresses);

        Task<int> DeleteEmployee(EmployeeModel employeeModel);
    }
}
