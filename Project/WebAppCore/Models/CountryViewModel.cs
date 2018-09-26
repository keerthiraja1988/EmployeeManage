using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Models
{
    public class CountryViewModel
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Int64 ModifiedBy { get; set; }
    }
}
