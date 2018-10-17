using CrossCutting.Caching;
using CrossCutting.IPRequest;
using CrossCutting.Logging;
using DomainModel;
using DomainModel.Shared;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace ServiceConcrete
{
    [NLogging]
    public class AppAnalyticsService : IAppAnalyticsService
    {
        private IAppAnalyticsRepository _IAppAnalyticsRepository;
        private IIPRequestDetails _IIPRequestDetails;

        public AppAnalyticsService(DbConnection Parameter, IIPRequestDetails iIPRequestDetails)
        {
            SqlInsightDbProvider.RegisterProvider();
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection");
            DbConnection c = new SqlConnection(sqlConnection);
            _IAppAnalyticsRepository = c.As<IAppAnalyticsRepository>();
            _IIPRequestDetails = iIPRequestDetails;
        }

        public Int32 UpdatedUserDisConnectionTracking(IpPropertiesModal ipAddressDetails)
        {
            return this._IAppAnalyticsRepository.UpdatedUserDisConnectionTracking(ipAddressDetails);
        }

        public IpPropertiesModal GetIpAddressDetails(string IpAddress)
        {
            if (IpAddress == "::1")
            {
                IpAddress = Faker.Internet.IPv4();
            }

            return _IIPRequestDetails.GetCountryDetailsByIP(IpAddress);
        }
    }
}