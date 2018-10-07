using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Shared
{
    public class CountryModel
    {

        public int CountryId { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int64 CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Int64 ModifiedBy { get; set; }

    }

}
