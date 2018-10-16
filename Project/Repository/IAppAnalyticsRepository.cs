using DomainModel;
using DomainModel.Shared;
using Insight.Database;
using System;
using System.Collections.Generic;

namespace Repository
{
    // ReSharper disable once IdentifierTypo
    public interface IAppAnalyticsRepository : IDisposable
    {
       
        [Sql("P_SaveIpAddressDetailsOnLogin")]      
        Int32 SaveIpAddressDetailsOnLogin(IpPropertiesModal ipAddressDetails);

        [Sql("P_UpdatedUserDisConnectionTracking")]
        Int32 UpdatedUserDisConnectionTracking(IpPropertiesModal ipAddressDetails);
    }
}
