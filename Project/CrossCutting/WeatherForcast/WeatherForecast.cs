using DarkSkyApi;
using DarkSkyApi.Models;
using DomainModel.DashBoard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CrossCutting.WeatherForcast
{
    public class WeatherForecast : IWeatherForecast
    {
        public WeatherModel GetWeatherForecastByCoOrdinates(string latitude, string longitude)
        {
            WeatherModel weatherModel = new WeatherModel();
            var client = new DarkSkyService("1edbf9d2042f997e9fe200cbfb8f55ae");
            Forecast result = client.GetWeatherDataAsync(
                                        Convert.ToDouble(latitude)
                                        , Convert.ToDouble(longitude)
                                        , Unit.UK).Result;

            weatherModel.Temperature = Math.Round(result.Currently.Temperature, 0).ToString() + "°C";
            weatherModel.WeatherConditionIcon = result.Currently.Icon.ToUpper().Replace("-", "_");
            weatherModel.WeatherCondition = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                                            result.Currently.Icon.ToUpper().Replace("-", " ")
                                            );
            weatherModel.WindSpeed = result.Currently.WindSpeed.ToString();
            weatherModel.Humidity = result.Currently.Humidity.ToString();

            return weatherModel;
        }
    }

    public interface IWeatherForecast
    {
        WeatherModel GetWeatherForecastByCoOrdinates(string latitude, string longitude);
    }
}
