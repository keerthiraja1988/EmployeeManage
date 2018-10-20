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
    public class ServiceBusService
    {
        public ServiceBusService(DbConnection Parameter)
        {
        }
    }
}