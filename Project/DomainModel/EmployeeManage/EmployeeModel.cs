using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.EmployeeManage
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Initial { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public int PermenantAddressId { get; set; }

        public int CurrentAddressId { get; set; }

        public string TIN { get; set; }

        public string PASSPORT { get; set; }

        public int WorkLocation { get; set; }

        public bool IsActive { get; set; }

        public bool IsAuthorized { get; set; }

        public Int64 UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Int64 ModifiedBy { get; set; }
    }
}