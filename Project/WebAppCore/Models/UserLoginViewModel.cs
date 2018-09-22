using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppCore.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Please Enter User Name")]
        [MaxLength(15, ErrorMessage = "Maximum User Name Length is 15 Characters")]
        [MinLength(5, ErrorMessage = "Minimum User Name Length is 5 Characters")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password Address")]
        [MinLength(8, ErrorMessage = "Minimum Password Length is 8 Characters")]
        [MaxLength(25, ErrorMessage = "Maximum Password Length is 25 Characters")]
        public string Password { get; set; }
    }
}