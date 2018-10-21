using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace WebAppCore
{
    public static class ApplicationGlobalVariables
    {
        public static readonly ConcurrentDictionary<string, ServiceMonitoring> ServiceScheduled =
 new ConcurrentDictionary<string, ServiceMonitoring>(StringComparer.InvariantCultureIgnoreCase);

        public static readonly System.Timers.Timer aTimer = new System.Timers.Timer(1000);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                // await SignalrHandler();

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    /* run your code here */
                    Console.WriteLine("Service Monitor Timer Started");

                    ApplicationGlobalVariables.aTimer.Elapsed += SignalrHandler;

                    ApplicationGlobalVariables.aTimer.Interval = 10000;

                    ApplicationGlobalVariables.aTimer.Enabled = true;
                }).Start();

                // logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .UseKestrel(c => c.AddServerHeader = false)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration() // IMPORTANT!!!

                .UseStartup<Startup>()
             .UseNLog() // NLog: setup NLog for Dependency injection
            ;

        public static void SignalrHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!SignalrHandler().Result)
            {
                ApplicationGlobalVariables.aTimer.Enabled = true;
            }
            else
            {
            }
            // return false;
        }

        public static async Task<bool> SignalrHandler()
        {
            bool connected = false;

            try
            {
                ServiceMonitoring s = new ServiceMonitoring();
                s.UpdatedOn = DateTime.Now;
                ApplicationGlobalVariables.ServiceScheduled.TryAdd("Test", s);

                var url = "http://localhost:8089";

                var _hubConnection = new HubConnection(url);
                var MarcolinMainProxy = _hubConnection.CreateHubProxy("MyHub");
                MarcolinMainProxy.On<string, string>("addMessage", InvokeMethod);

                await _hubConnection.Start();

                if (_hubConnection.State == ConnectionState.Connected)
                {
                    Debug.WriteLine("Hub Connected");
                    connected = true;
                    _hubConnection.Closed += Connection_Closed;
                    ApplicationGlobalVariables.aTimer.Enabled = false;
                    return connected;
                }
                else
                {
                    return connected;
                }

                //if (_hubConnection.State == ConnectionState.Disconnected)
                //{
                //    Connection_Closed();
                //}
            }
            catch (Exception)
            {
                return connected;
            }
        }

        private static void Connection_Closed()
        {
            // specify a retry duration
            TimeSpan retryDuration = TimeSpan.FromDays(30);
            DateTime retryTill = DateTime.UtcNow.Add(retryDuration);

            while (DateTime.UtcNow < retryTill)
            {
                bool connected = SignalrHandler().Result;
                if (connected)
                    return;

                Thread.Sleep(2000);
            }
            Debug.WriteLine("Connection closed");
        }

        private static void InvokeMethod(string type, string type1)
        {
            Debug.WriteLine(String.Format("Recieved Message From Server On :{0}", System.DateTime.Now.ToString()));
            Debug.WriteLine("Message Received");
            ServiceMonitoring serviceMonitoring = new ServiceMonitoring();
            serviceMonitoring.UpdatedOn = DateTime.Now;

            var vvv = ApplicationGlobalVariables.ServiceScheduled.Where(p => p.Key == "Test").Select(s => s.Value).FirstOrDefault();

            ApplicationGlobalVariables.ServiceScheduled.TryUpdate("Test", serviceMonitoring, vvv);
        }
    }

    public class ServiceMonitoring
    {
        public string ServiceName { get; set; }
        public Int64 ServiceId { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}