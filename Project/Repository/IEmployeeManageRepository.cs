using DomainModel.EmployeeManage;
using Insight.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
   public interface IEmployeeManageRepository
    {
        [Sql("P_CreateEmployee")]
        bool LoadEmployeeData(EmployeeModel EmployeeDetail, List<EmployeeAddressModel> EmployeeAddress);
       

    }
}
