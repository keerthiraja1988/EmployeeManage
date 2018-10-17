using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class UserRolesModel
    {
        public Int32 UserId { get; set; }
        public Int32 RoleId { get; set; }
        public string RoleName { get; set; }

        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        public Int64 ModifiedBy { get; set; }
    }
}