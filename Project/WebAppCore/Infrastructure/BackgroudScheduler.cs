using Hangfire;
using Kendo.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using RazorLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAppCore.Areas.DashBoard.Models;
using WebAppCore.Areas.DashBoard.SignalR;
using WebAppCore.SignalRHubs;

namespace WebAppCore.Infrastructure
{

    public class CheckStudentAgeJob
    {
        
        public void Execute()
        {
          
        }
    }

    internal class TimedHostedService :  IHostedService, IDisposable
    {
        private readonly IHubContext<DashBoardHub> _hubContext;

        
        private readonly ILogger _logger;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger
            , IHubContext<DashBoardHub> hubContext)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            DashBoardWidgetsDTO dashBoardWidgetsDTO = new DashBoardWidgetsDTO();
            dashBoardWidgetsDTO.DashBoardRow1Widgets = new DashBoardRow1Widgets();
            dashBoardWidgetsDTO.DashBoardRow1Widgets.TotalNoOfEmployees = 
                Convert.ToString(DateTime.Now.Hour.ToString()).ToString()
                  + " "  + DateTime.Now.Minute.ToString() + " " +  DateTime.Now.Second.ToString();
            dashBoardWidgetsDTO.DashBoardRow1Widgets.NoOfEmployeesCreatedToday = "25";
            dashBoardWidgetsDTO.DashBoardRow1Widgets.NoOfEmployeesPendingAuth = "37";

            var vv = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory()));
            string content;
            using (StreamReader sr = new StreamReader(vv.Root + "Areas/DashBoard/Views/DashBoard/_DashBoardRow1Widgets.cshtml"))
            {
                content = sr.ReadToEnd();
            }

            // string template = "Hello @Model.Name! Welcome to Razor!";


            var engine = new RazorLightEngineBuilder()
               
               .UseMemoryCachingProvider()
               .Build();

        

            var getdashBoardRow1DetailsTask = Task.Run(() =>
                                      engine.CompileRenderAsync("sdvsdvd", content,
                                                    dashBoardWidgetsDTO.DashBoardRow1Widgets)
                                            );

            getdashBoardRow1DetailsTask.Wait();

            string pVdashBoardRow1WidgetsHtml = getdashBoardRow1DetailsTask.Result;


            _hubContext.Clients.All.SendAsync("rUpdateDashBoardRow1Widgets", pVdashBoardRow1WidgetsHtml);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

    internal class ViewModel
    {
        public string Name { get; internal set; }
    }
}
