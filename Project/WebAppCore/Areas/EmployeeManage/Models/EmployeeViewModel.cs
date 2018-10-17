using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.EmployeeManage.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [MaxLength(25, ErrorMessage = "Maximum First Name Length is 25 Characters")]
        [MinLength(3, ErrorMessage = "Minimum First Name Length is 3 Characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [MaxLength(25, ErrorMessage = "Maximum Last Name Length is 25 Characters")]
        [MinLength(3, ErrorMessage = "Minimum Last Name Length is 3 Characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Initial { get; set; }

        [Required(ErrorMessage = "Please Enter Email Id")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
      ErrorMessage = "Please Enter Email Address in emailAddress@mail.com")]
        public string Email { get; set; }

        [Display(Name = "Country Code")]
        [Required(ErrorMessage = "Please Provide Country Code")]
        public string CountryCode { get; set; }

        [Display(Name = "Mobile No")]
        [Required(ErrorMessage = "Please Enter Mobile No")]
        public string MobileNo { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Please Provide Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Date Of Joining")]
        [Required(ErrorMessage = "Please Provide Date Of Joining")]
        public DateTime? DateOfJoining { get; set; }

        public int PermenantAddressId { get; set; }

        public int CurrentAddressId { get; set; }

        [Display(Name = "PAN No.")]
        [Required(ErrorMessage = "Please Provide PAN No.")]
        [MaxLength(15, ErrorMessage = "Maximum Last Name Length is 15 Characters")]
        [MinLength(3, ErrorMessage = "Minimum Last Name Length is 3 Characters")]
        public string TIN { get; set; }

        [Display(Name = "Passport No.")]
        [Required(ErrorMessage = "Please Provide Passport No.")]
        [MaxLength(15, ErrorMessage = "Maximum Last Name Length is 15 Characters")]
        [MinLength(3, ErrorMessage = "Minimum Last Name Length is 3 Characters")]
        public string PASSPORT { get; set; }

        [Display(Name = "Work Location")]
        [Required(ErrorMessage = "Please Provide Work Location")]
        public int WorkLocation { get; set; }

        public bool IsActive { get; set; }

        public bool IsAuthorized { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Int64 ModifiedBy { get; set; }

        public EmployeeAddressViewModel CurrentAddress { get; set; }

        public EmployeeAddressViewModel PermenantAddress { get; set; }
    }
}