using Autofac;
using CrossCutting.Caching;
using CrossCutting.EmailService;
using CrossCutting.Logging;
using DomainModel;
using MassTransit;
using MessageContracts;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Autofac;
using static DependencyInjecionResolver.DependencyInjecionResolver;

namespace SendEmail
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Signal R Serivce Started");
                List<ApplicationConfigModel> applicationConfigs = new List<ApplicationConfigModel>();

                applicationConfigs.Add(new ApplicationConfigModel
                {
                    Key = "DBConnection"
                    ,
                    Value = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True"
                });

                Caching.Instance.AddApplicationConfigs(applicationConfigs);

                ConfigureService.Configure();
                Console.ReadLine();
            }
        }

        private static void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

            context.Clients.All.addMessage("", "SendEmail Service BroadCasted on " + DateTime.Now);
        }
    }

    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<SendEmailService>(service =>
                {
                    service.ConstructUsing(s => new SendEmailService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.
                configure.RunAsLocalSystem();
                configure.SetServiceName("MyWindowServiceWithTopshelf");
                configure.SetDisplayName("MyWindowServiceWithTopshelf");
                configure.SetDescription("My .Net windows service with Topshelf");
            });
        }
    }

    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class MyHub : Hub
    {
        public void SendMessage(string name, String message)

        {
            // Call the addMessage methods on all clients

            Clients.All.addMessage("dddd", message);
        }

        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}