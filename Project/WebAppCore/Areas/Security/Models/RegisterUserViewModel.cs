using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.Security.Models
{
    public class RegisterUserViewModel
    {

        [Required(ErrorMessage = "Please Enter User Name")]
        [MaxLength(25, ErrorMessage = "Maximum User Name Length is 25 Characters")]
        [MinLength(5, ErrorMessage = "Minimum User Name Length is 5 Characters")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [MaxLength(25, ErrorMessage = "Maximum First Name Length is 25 Characters")]
        [MinLength(4, ErrorMessage = "Minimum First Name Length is 4 Characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [MaxLength(25, ErrorMessage = "Maximum Last Name Length is 25 Characters")]
        [MinLength(4, ErrorMessage = "Minimum Last Name Length is 4 Characters")]

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
      ErrorMessage = "Please Enter Email Address : fakeEmail@fake.com")]
        [MaxLength(35, ErrorMessage = "Maximum Email Length is 35 Characters")]
        public string Email { get; set; }

        [Remote(action: "Foo", controller: "Base", areaName: "Base", ErrorMessage = "Remote validation is working")]
        [Required(ErrorMessage = "Please Enter Password")]
        [MinLength(8, ErrorMessage = "Minimum Password Length is 8 Characters")]
        [MaxLength(25, ErrorMessage = "Maximum Password Length is 25 Characters")]
      //  [RegularExpression(User.PasswordRegularExpression, ErrorMessage = "Пароль может содержать только латинские символы, дефисы, подчеркивания, точки")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Re-Type Password")]
        [Compare("Password", ErrorMessage = "Password Mismatch, Please Correct")]
        [MaxLength(25, ErrorMessage = "Maximum Password Length is 25 Characters")]
        public string ReTypePassword { get; set; }

        public string CryptLoginPassword { get; set; }

        public string CapthaValue { get; set; }
        public string CapthaEncValue { get; set; }



    }
}
