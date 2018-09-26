using Microsoft.Extensions.Configuration;
using PostSharp.Aspects;
using PostSharp.Serialization;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using CrossCutting.Caching;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Data;
using Serilog.Events;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CrossCutting.Logging
{
    [PSerializable]

    public class NlogTraceAttribute : OnMethodBoundaryAspect
    {


        /// <summary>  
        /// On Method Entry  
        /// </summary>  
        /// <param name="args"></param>  
        public override void OnEntry(MethodExecutionArgs args)
        {
            SeriLogSQL.LogInformation();

            Controller apiController = (Controller)args.Instance;
            //var context = apiController.;
           // HttpRequestHeaders headers = context.Request.Headers;

            // Use Web API request header info here
          //  string headerText = headers.GetValues("MyHeader").First();

            //var vvv = Caching.Caching.Instance.GetApplicationConfigs();

            // string connectionString1 = ConfigurationManager.ConnectionStrings["KeyToOverride"].ToString();

            var methodName = GetMethodName(args.Method);
            args.MethodExecutionTag = methodName;

            var argument = args.Arguments;
            var argument1 = args.Method;

            var columnOption = new ColumnOptions
            {
                AdditionalDataColumns = new Collection<DataColumn>
    {
       // new DataColumn {DataType = typeof(string), ColumnName = "UserName"},
       //new DataColumn {DataType = typeof(string), ColumnName = "UserId"},
       // new DataColumn {DataType = typeof(string), ColumnName = "UserHostAddress"},
       // new DataColumn {DataType = typeof(string), ColumnName = "RequestUrl"},
       // new DataColumn {DataType = typeof(string), ColumnName = "BrowserAgent"},

    }
            };

            var connectionString = @"Data Source=.;Initial Catalog=DB_A40D34_keerthirajapraba;Integrated Security=True";  // or the name of a connection string in the app config
            var tableName = "Log";
          //  var columnOptions = columnOption;
            var columnOptions = new ColumnOptions();  // optional

            using (var log = new LoggerConfiguration()
            .WriteTo.MSSqlServer(connectionString, tableName, columnOptions: columnOptions)
            .CreateLogger())
            {

                //log.ForContext("UserName", "Test Data").Information("Index method called!!!");

                var sensorInput = new { Latitude = 25, Longitude = 134 };
                log.Information("Processing {@SensorInput}", sensorInput);

                log.Information("Hello, Serilog!"
                    );

                log.Warning("Goodbye, Serilog.");
            }



            //throw new Exception();
        }

        /// <summary>  
        /// On Method success  
        /// </summary>  
        /// <param name="args"></param>  
        public override void OnSuccess(MethodExecutionArgs args)
        {

        }


        /// <summary>  
        /// On Method Exception  
        /// </summary>  
        /// <param name="args"></param>  
        public override void OnException(MethodExecutionArgs args)
        {

        }

        /// <summary>  
        /// On Method Exit  
        /// </summary>  
        /// <param name="args"></param>  
        public override void OnExit(MethodExecutionArgs args)
        {
        }

        private string GetMethodName(MethodBase method)
        {
            if (method.IsGenericMethod)
            {
                var genericArgs = method.GetGenericArguments();
                var typeNames = genericArgs.Select(t => t.Name);
                return string.Format("{0}<{1}>", method.Name, String.Join(",", typeNames));
            }
            return method.Name;
        }

        static string DisplayObjectInfo(MethodExecutionArgs args)
        {
            StringBuilder sb = new StringBuilder();
            Type type = args.Arguments.GetType();
            sb.Append("Method " + args.Method.Name);
            sb.Append("\r\nArguments:");
            FieldInfo[] fi = type.GetFields();

            if (fi.Length > 0)
            {
                foreach (FieldInfo f in fi)
                {
                    sb.Append("\r\n " + f + " = " + f.GetValue(args.Arguments));
                }
            }
            else
                sb.Append("\r\n None");

            return sb.ToString();
        }
    }
}
