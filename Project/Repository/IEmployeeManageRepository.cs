using DomainModel.EmployeeManage;
using Insight.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
   public interface IEmployeeManageRepository
    {
        [Sql("P_CreateEmployee")]
        bool LoadEmployeeData(EmployeeModel EmployeeDetail, List<EmployeeAddressModel> EmployeeAddress);

        [Sql("P_GetEmployeesDetails")]
        Task<List<EmployeeModel>> GetEmployeesDetails();

        [Sql("P_GetEmployeesDetailsForSearch")]
        Task<List<EmployeeSearchModel>> GetEmployeesDetailsForSearch(EmployeeSearchModel employeeSearchModel);

        [Sql("P_GetEmployeesDetailsOnSearch")]
        Task<List<EmployeeModel>> GetEmployeesDetailsOnSearch(EmployeeSearchModel employeeSearchModel);

    }
}
