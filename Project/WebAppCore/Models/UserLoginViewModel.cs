using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppCore.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Please Enter User Name")]
       
        [Display(Name = "User Name")]
        public string LoginUserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password Address")]
        [Display(Name = "Password")]
        public string LoginPassword { get; set; }
    }
}