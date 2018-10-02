using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.EmployeeManage.Models
{
    public class EmployeeSearchViewModel
    {
        public string EmployeeId { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Initial { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirthStart { get; set; }
        public DateTime DateOfBirthEnd { get; set; }


        public DateTime DateOfJoiningStart { get; set; }
        public DateTime DateOfJoiningStartEnd { get; set; }

        public string TIN { get; set; }

        public string PASSPORT { get; set; }

        public int WorkLocation { get; set; }

        public bool IsActive { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
