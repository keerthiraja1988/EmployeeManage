using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{


    public class UserAccountViewModel
    {
        public Int64 UserId { get; set; }

        public Int64 RequestUserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<UserRoleViewModel> Roles { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string LoginStatus { get; set; }

        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }

        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        public Int64 ModifiedBy { get; set; }
    }

    public class UserRoleViewModel
    {
        public string Role { get; set; }

        public int UserId { get; set; }

        public string RoleName { get; set; }

        public int RoleId { get; set; }

        public bool RoleActive { get; set; }

    }
}
