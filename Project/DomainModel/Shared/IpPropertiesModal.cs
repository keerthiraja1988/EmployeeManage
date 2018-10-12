using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Shared
{
   public class IpPropertiesModal
    {
        public string Status { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string TimeZone { get; set; }
        public string ISP { get; set; }
        public string ORG { get; set; }
        public string ISPDetails { get; set; }
        public string Query { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int64 CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Int64 ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }

        public Int64 UserId { get; set; }
        public string UserName { get; set; }

        public bool IsLoginSuccess { get; set; }


    }
}
