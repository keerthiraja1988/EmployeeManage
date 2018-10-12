using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppCore.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Please Enter User Name")]
       
        [Display(Name = "User Name")]
        public string LoginUserName { get; set; }

        //[Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        public string LoginPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string CryptLoginPassword { get; set; }

        public string UserIpAddress { get; set; }

    }
}