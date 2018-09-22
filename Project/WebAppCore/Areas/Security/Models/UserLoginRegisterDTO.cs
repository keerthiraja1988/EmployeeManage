using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Models;

namespace WebAppCore.Areas.Security.Models
{
    public class UserLoginRegisterDTO
    {
        public RegisterUserViewModel RegisterUserViewModel { get; set; }
        public UserLoginViewModel UserLoginViewModel { get; set; }
    }
}
