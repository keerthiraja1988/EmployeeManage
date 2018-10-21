using Autofac;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DependencyInjecionResolver.DependencyInjecionResolver;

namespace SendEmailService
{
    public class SendEmailService
    {
        public void Start()
        {
            Console.WriteLine("Service Bus Started.");

            var builder = new ContainerBuilder();

            // register a specific consumer
            builder.RegisterType<ProcessEmailRequest>();

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
                        ec.Consumer<ProcessEmailRequest>(context);
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
}