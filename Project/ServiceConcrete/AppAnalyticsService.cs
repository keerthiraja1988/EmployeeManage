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

   
    // ReSharper disable once UnusedMember.Global
    public class AppAnalyticsService : IAppAnalyticsService
    {
        // ReSharper disable once IdentifierTypo
        private readonly IAppAnalyticsRepository _iAppAnalyticsRepository;
        private readonly IIpRequestDetails _iipRequestDetails;

        // ReSharper disable once IdentifierTypo
        public AppAnalyticsService(DbConnection parameter, IIpRequestDetails iIpRequestDetails)
        {
            SqlInsightDbProvider.RegisterProvider();
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection");
            DbConnection c = new SqlConnection(sqlConnection);
            _iAppAnalyticsRepository = c.As<IAppAnalyticsRepository>();
            _iipRequestDetails = iIpRequestDetails;
        }

        public Int32 UpdatedUserDisConnectionTracking(IpPropertiesModal ipAddressDetails)
        {
            return this._iAppAnalyticsRepository.UpdatedUserDisConnectionTracking(ipAddressDetails);
        }

        public IpPropertiesModal GetIpAddressDetails(string ipAddress)
        {
            return _iipRequestDetails.GetCountryDetailsByIp(ipAddress);
        }
    }
}
