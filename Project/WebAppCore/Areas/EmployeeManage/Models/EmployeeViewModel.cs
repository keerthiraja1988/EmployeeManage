using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.EmployeeManage.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Initial { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfJoining { get; set; }

        public int PermenantAddressId { get; set; }

        public int CurrentAddressId { get; set; }

        public string TIN { get; set; }

        public string PASSPORT { get; set; }

        public int WorkLocation { get; set; }

        public bool IsActive { get; set; }

        public bool IsAuthorized { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Int64 ModifiedBy { get; set; }
    }
}
