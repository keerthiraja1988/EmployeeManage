using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.EmployeeManage.Models
{
    public class EmployeeAddressViewModel
    {
        public int EmployeeAddressId { get; set; }

        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please Enter Address Line 1")]
        [MaxLength(40, ErrorMessage = "Maximum Address Line 1 Length is 40 Characters")]
        [MinLength(3, ErrorMessage = "Minimum Address Line 1 Length is 3 Characters")]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "Please Enter Address Line 2")]
        [MaxLength(40, ErrorMessage = "Maximum Address Line 2 Length is 40 Characters")]
        [MinLength(3, ErrorMessage = "Minimum Address Line 2 Length is 3 Characters")]
        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        [Display(Name = "Address Line 3")]
        public string Address3 { get; set; }

        [Required(ErrorMessage = "Please Enter City Name")]
        [MaxLength(25, ErrorMessage = "Maximum City Name Length is 40 Characters")]
        [MinLength(3, ErrorMessage = "Minimum City Name Length is 3 Characters")]
        [Display(Name = "City Name")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [MaxLength(25, ErrorMessage = "Maximum State Length is 40 Characters")]
        [MinLength(3, ErrorMessage = "Minimum State Length is 3 Characters")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Provide Country")]      
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public string AddressType { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Int64 ModifiedBy { get; set; }
    }
}
