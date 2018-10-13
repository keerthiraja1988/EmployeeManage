using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.DashBoard.Models
{
    public class WeatherWidgetsViewModel
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
