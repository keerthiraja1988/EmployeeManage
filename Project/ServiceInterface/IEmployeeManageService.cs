using DomainModel.EmployeeManage;
using DomainModel.Shared;
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

        Task<List<CountryModel>> GetCountries();

        Task<int> AddEmployee(EmployeeModel employeeModel, List<EmployeeAddressModel> employeeAddresses);
    }
}
