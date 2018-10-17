using DomainModel;
using DomainModel.Shared;
using Insight.Database;
using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IAppAnalyticsRepository : IDisposable
    {
        [Sql("P_SaveIpAddressDetailsOnLogin")]
        Int32 SaveIpAddressDetailsOnLogin(IpPropertiesModal IpAddressDetails);

        [Sql("UpdatedUserDisConnectionTracking")]
        Int32 UpdatedUserDisConnectionTracking(IpPropertiesModal IpAddressDetails);
    }
}