using Autofac;
using MassTransit;
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
using static DependencyInjecionResolver.DependencyInjecionResolver;

namespace SendEmailService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string url = "http://localhost:8089";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                ServiceBroadCastThread();

                ConfigureService.Configure();
                Console.ReadLine();
            }
        }

        private static void ServiceBroadCastThread()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                Console.WriteLine("Service Monitor Timer Started");

                var aTimer = new System.Timers.Timer(1000);

                aTimer.Elapsed += aTimer_Elapsed;

                aTimer.Interval = 3000;

                aTimer.Enabled = true;
            }).Start();
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

    public class MyService
    {
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
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}