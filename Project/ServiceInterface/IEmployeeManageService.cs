using DomainModel.EmployeeManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterface
{
  public  interface IEmployeeManageService
    {
        List<EmployeeModel> LoadEmployeeData();
    }
}
