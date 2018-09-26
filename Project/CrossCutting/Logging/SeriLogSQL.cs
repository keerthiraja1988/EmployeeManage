using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Logging
{
    public static class SeriLogSQL
    {
        static Logger Log;

        public static void  LogInformation()
        {
            Log.ForContext("UserName", "Test Data").Information("Index method called!!!");

            var sensorInput = new { Latitude = 25, Longitude = 134 };
            Log.Information("Processing {@SensorInput}", sensorInput);


            Log.Warning("Goodbye, Serilog.");
        }

        static SeriLogSQL()
        {
            var columnOption = new ColumnOptions
            {
                AdditionalDataColumns = new Collection<DataColumn>
    {
        new DataColumn {DataType = typeof(string), ColumnName = "UserName"},
       new DataColumn {DataType = typeof(string), ColumnName = "UserId"},
        new DataColumn {DataType = typeof(string), ColumnName = "UserHostAddress"},
        new DataColumn {DataType = typeof(string), ColumnName = "RequestUrl"},
        new DataColumn {DataType = typeof(string), ColumnName = "BrowserAgent"},

    }
            };

            var connectionString = @"Data Source=.;Initial Catalog=DB_A40D34_keerthirajapraba;Integrated Security=True";  // or the name of a connection string in the app config
            var tableName = "Log";
            //var columnOptions = columnOption;
            var columnOptions = new ColumnOptions();  // optional

            Log = new LoggerConfiguration()
            .WriteTo.MSSqlServer(connectionString, tableName, columnOptions: columnOptions)
            .CreateLogger();




        }
    }
}

