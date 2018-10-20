using Autofac;
using CrossCutting.Caching;
using CrossCutting.EmailService;
using CrossCutting.Logging;
using DomainModel;
using MassTransit;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DependencyInjecionResolver.DependencyInjecionResolver;

namespace SendEmail
{
    [NLogging]
    public class SendEmailService
    {
        public bool Start()
        {
            Console.WriteLine("Service Bus Started.");

            var builder = new ContainerBuilder();

            // register a specific consumer
            builder.RegisterType<ProcessRequest>();

            // just register all the consumers
            builder.RegisterConsumers(Assembly.GetExecutingAssembly());

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

            bc.Start();
            try
            {
                Console.WriteLine("Service Bus Ready.");

                Console.WriteLine("WebApp.Start(url)");

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

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                bc.Stop();
            }

            return true;
        }

        public bool Stop()
        {
            return true;
        }

        private static void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

            context.Clients.All.addMessage("", "SendEmail Service BroadCasted on " + DateTime.Now);
        }
    }
}