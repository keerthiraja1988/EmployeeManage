using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.UserAccount
{
    public class LoggedInUserDetailsModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> UserRoles { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string IpAddress { get; set; }
        public string ISPDetails { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public DateTime? SessionDisconnectedOn { get; set; }
    }
}
