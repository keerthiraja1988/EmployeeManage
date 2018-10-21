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
                configure.Service<MyService>(service =>
                {
                    service.ConstructUsing(s => new MyService());
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
        public void Start()
        {
            //var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            //{
            //    var host = sbc.Host(new Uri("rabbitmq://localhost:/"), h =>
            //    {
            //        h.Username("guest");
            //        h.Password("guest");
            //    });

            //    sbc.ReceiveEndpoint(host, "NewEmployeeRegisterService", endpoint =>
            //    {
            //        endpoint.Consumer<ProcessRequest>();
            //    });
            //});

            //bus.StartAsync();
            //Console.WriteLine("Service Bus Ready.");
            ////bus.Publish(new MyMessage { Value = "Hello, World." });

            //Console.ReadLine();

            //bus.StopAsync();

            Console.WriteLine("Service Bus Started.");

            var builder = new ContainerBuilder();

            // register a specific consumer
            builder.RegisterType<ProcessRequest>();

            var sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";

            builder.RegisterModule(new ServiceDIContainer(sqlConnection));

            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("NewEmployeeRegisterService", ec =>
                    {
                        // otherwise, be smart, register explicitly
                        ec.Consumer<ProcessRequest>(context);
                    });
                });

                return busControl;
            })
            .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            var container = builder.Build();

            var bc = container.Resolve<IBusControl>();

            bc.StartAsync();
            Console.WriteLine("Service Bus Ready.");
            //bus.Publish(new MyMessage { Value = "Hello, World." });

            Console.ReadLine();

            bc.StopAsync();
        }

        public void Stop()
        {
            // write code here that runs when the Windows Service stops.
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
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}