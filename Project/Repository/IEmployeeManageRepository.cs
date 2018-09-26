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
    }
}
