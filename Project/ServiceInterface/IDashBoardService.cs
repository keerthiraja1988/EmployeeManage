using DomainModel;
using DomainModel.DashBoard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IDashBoardService
    {
        Task<DashBoardRow1WidgetsModel> GetBoardRow1WidgetsDetails();

        Task<WeatherModel> GetCurrentWeatherDetails(string userIpAddress);

    }
}
