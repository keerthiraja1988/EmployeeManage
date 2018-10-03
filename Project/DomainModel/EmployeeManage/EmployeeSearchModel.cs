using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.EmployeeManage
{
    public class EmployeeSearchModel
    {

        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }


        public string SearchText { get; set; }
        public string SearchColumn { get; set; }

        public DateTime DateOfBirthStart { get; set; }
        public DateTime DateOfBirthEnd { get; set; }

        public DateTime DateOfJoiningStart { get; set; }
        public DateTime DateOfJoiningStartEnd { get; set; }

    }
}
