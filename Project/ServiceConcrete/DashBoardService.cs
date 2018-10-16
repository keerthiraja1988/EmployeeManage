using CrossCutting.Caching;
using CrossCutting.IPRequest;
using CrossCutting.Logging;
using DomainModel.DashBoard;
using FizzWare.NBuilder;
using Insight.Database;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting.WeatherForecast;

namespace ServiceConcrete
{
    [NLogging]
    public class DashBoardService : IDashBoardService
    {
        private readonly IWeatherForecast _iWeatherForecast;
        private readonly IIpRequestDetails _iipRequestDetails;

        public DashBoardService(DbConnection parameter, IWeatherForecast iWeatherForecast,
            IIpRequestDetails iIpRequestDetails)
        {
            SqlInsightDbProvider.RegisterProvider();
            //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection")
                                           ;
            DbConnection c = new SqlConnection(sqlConnection);

            _iipRequestDetails = iIpRequestDetails;

            _iWeatherForecast = iWeatherForecast;
        }

        public Task<DashBoardRow1WidgetsModel> GetBoardRow1WidgetsDetails()
        {
            var dashBoardRow1WidgetsModels = Builder<DashBoardRow1WidgetsModel>.CreateListOfSize(1)
               .All()
           .With(c => c.TotalNoOfApplicationUsers = Faker.Number.RandomNumber(300, 400).ToString())
           .With(c => c.NoOfActiveSessions = Faker.Number.RandomNumber(45, 55).ToString())
           .With(c => c.NoOfActiveUsers = Faker.Number.RandomNumber(15, 30).ToString())
           .With(c => c.NoOfEmployeesCreatedToday = Faker.Number.RandomNumber(25, 40).ToString())
            .With(c => c.NoOfEmployeesPendingAuth = Faker.Number.RandomNumber(35, 56).ToString())
             .With(c => c.TodayNoOfApplicationErrors = Faker.Number.RandomNumber(100, 150).ToString())
            .With(c => c.ErrorLastLoggedOn = Faker.Date.PastWithTime().ToString(@"yyyy-MM-dd hh:mm tt", new CultureInfo("en-US")))
            .With(c => c.ServicesLastChecked = Faker.Date.PastWithTime().ToString(@"yyyy-MM-dd hh:mm tt", new CultureInfo("en-US")))
            .With(c => c.TotalNoOfApplicationErrors = Faker.Number.RandomNumber(5000, 6000).ToString())
            .With(c => c.TotalNoOfEmployees = Faker.Number.RandomNumber(2500, 3580).ToString())
            .With(c => c.TotalNoOfServicesDown = Faker.Number.RandomNumber(0, 5).ToString())
            .With(c => c.TotalNoOfServicesScheduled = Faker.Number.RandomNumber(10, 20).ToString())
           .Build();

            var getDashBoardRow1DetailsTask = Task.Run(() =>
                                   dashBoardRow1WidgetsModels.FirstOrDefault()
                                          );
            getDashBoardRow1DetailsTask.Wait();

            return getDashBoardRow1DetailsTask;
        }

        public Task<WeatherModel> GetCurrentWeatherDetails(string userIpAddress)
        {
            var ipPropertiesModal = _iipRequestDetails.GetCountryDetailsByIp(userIpAddress);

            var getDashBoardRow1DetailsTask = Task.Run(() =>
                              _iWeatherForecast.GetWeatherForecastByCoOrdinates(
                                  ipPropertiesModal.Lat
                                  , ipPropertiesModal.Lon)
                                      );
            getDashBoardRow1DetailsTask.Wait();
            getDashBoardRow1DetailsTask.Result.CurrentCityName =
                        ipPropertiesModal.City;

            getDashBoardRow1DetailsTask.Result.CurrentCountryName = ipPropertiesModal.CountryCode
                                        + ", " + ipPropertiesModal.Country;
            return getDashBoardRow1DetailsTask;
        }
    }
}