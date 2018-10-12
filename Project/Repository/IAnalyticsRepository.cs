using DomainModel;
using DomainModel.Shared;
using Insight.Database;
using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IAnalyticsRepository : IDisposable
    {
       
        [Sql("P_SaveIpAddressDetailsOnLogin")]      
        Int32 SaveIpAddressDetailsOnLogin(IpPropertiesModal IpAddressDetails);
    }
}
