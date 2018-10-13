using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.Security.Models
{
    public class LoggedInUserDetailsViewModel
    {


        public LoggedInUserDetailsViewModel CurrentLoggedInUserDetailsViewModel { get; set; }
        public LoggedInUserDetailsViewModel LastLoggedInUserDetailsViewModel { get; set; }

        public Guid CookieUniqueId { get; set; }
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
