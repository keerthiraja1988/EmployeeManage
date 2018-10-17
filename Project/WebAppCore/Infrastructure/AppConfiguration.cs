using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Infrastructure
{
    public class AppConfiguration
    {
        private static AppConfiguration _appConfiguration;

        public string AppConnection { get; set; }

        public AppConfiguration(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.AppConnection = config.GetValue<string>("DBConnection");

            // Now set Current
            _appConfiguration = this;
        }

        public static AppConfiguration Current
        {
            get
            {
                if (_appConfiguration == null)
                {
                    _appConfiguration = GetCurrentSettings();
                }

                return _appConfiguration;
            }
        }

        public static AppConfiguration GetCurrentSettings()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new AppConfiguration(configuration.GetSection("DBConnection"));

            return settings;
        }
    }
}