using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.DashBoard
{
    public class WeatherModel
    {
        public string CurrentCityName { get; set; }
        public string CurrentCountryName { get; set; }

        public string Temperature { get; set; }
        public string WeatherCondition { get; set; }
        public string WeatherConditionIcon { get; set; }
        public string Humidity { get; set; }
        public string WindSpeed { get; set; }
    }
}