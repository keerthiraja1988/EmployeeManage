using AutoMapper;
using DomainModel.DashBoard;
using Kendo.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RazorLight;
using ServiceInterface;
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
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly IHubContext<DashBoardHub> _hubContext;

        private Timer _timer;
        private IDashBoardService _iDashBoardService;
        private readonly IMapper _mapper;

        public TimedHostedService(ILogger<TimedHostedService> logger
            , IHubContext<DashBoardHub> hubContext, IDashBoardService IDashBoardService
            , IMapper mapper)
        {
            _mapper = mapper;
            _hubContext = hubContext;
            _iDashBoardService = IDashBoardService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(20));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            DashBoardRow1WidgetsModel dashBoardRow1WidgetsModel = new DashBoardRow1WidgetsModel();
            var dashBoardRow1WidgetsModelTask = Task.Run(() =>
                                    this._iDashBoardService.GetBoardRow1WidgetsDetails()
                                );
            string content;
            var vv = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory()));

            using (StreamReader sr = new StreamReader(vv.Root + "Areas/DashBoard/Views/DashBoard/_DashBoardRow1Widgets.cshtml"))
            {
                content = sr.ReadToEnd();
            }

            var engine = new RazorLightEngineBuilder()
               .UseMemoryCachingProvider()
               .Build();

            dashBoardRow1WidgetsModelTask.Wait();
            dashBoardRow1WidgetsModel = dashBoardRow1WidgetsModelTask.Result;

            DashBoardWidgetsDTO dashBoardWidgetsDTO = new DashBoardWidgetsDTO();
            dashBoardWidgetsDTO.DashBoardRow1WidgetsViewModel = new DashBoardRow1WidgetsViewModel();
            dashBoardWidgetsDTO.DashBoardRow1WidgetsViewModel = _mapper.Map<DashBoardRow1WidgetsViewModel>(dashBoardRow1WidgetsModel);

            var getdashBoardRow1DetailsTask = Task.Run(() =>
                                      engine.CompileRenderAsync("sdvsdvd", content,
                                                    dashBoardWidgetsDTO.DashBoardRow1WidgetsViewModel)
                                            );

            getdashBoardRow1DetailsTask.Wait();

            string pVdashBoardRow1WidgetsHtml = getdashBoardRow1DetailsTask.Result;

            _hubContext.Clients.All.SendAsync("rUpdateDashBoardRow1Widgets", pVdashBoardRow1WidgetsHtml);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
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