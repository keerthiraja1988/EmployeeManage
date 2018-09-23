using Microsoft.Extensions.Configuration;
using PostSharp.Aspects;
using PostSharp.Serialization;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using CrossCutting.Caching;

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
            //var vvv = Caching.Caching.Instance.GetApplicationConfigs();

            // string connectionString1 = ConfigurationManager.ConnectionStrings["KeyToOverride"].ToString();

            var argument = args.Arguments;
            var argument1 = args.Method;

            var connectionString = @"Data Source=.;Initial Catalog=DB_A40D34_keerthirajapraba;Integrated Security=True";  // or the name of a connection string in the app config
            var tableName = "Logs";
            var columnOptions = new ColumnOptions();  // optional

            using (var log = new LoggerConfiguration()
            .WriteTo.MSSqlServer(connectionString, tableName, columnOptions: columnOptions)
            .CreateLogger())
            {
                log.Information("Hello, Serilog!");

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

    }
}
