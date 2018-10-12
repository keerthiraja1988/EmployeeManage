using DarkSkyApi;
using DarkSkyApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrossCutting.WeatherForcast
{
    public class WeatherForecast
    {
        public dynamic GetWeatherForecastByCoOrdinates()
        {
            var client = new DarkSkyService("1edbf9d2042f997e9fe200cbfb8f55ae");
            Forecast result = client.GetWeatherDataAsync(13.0833, 80.2833, Unit.UK).Result;
            return result;
        }
    }
}
