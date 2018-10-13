using DomainModel;
using DomainModel.DashBoard;
using DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IAppAnalyticsService
    {
        Int32 UpdatedUserDisConnectionTracking(IpPropertiesModal ipAddressDetails);

        IpPropertiesModal GetIpAddressDetails(string IpAddress);
    }
}
