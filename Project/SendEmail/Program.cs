using CrossCutting.EmailService;
using MassTransit;
using MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://localhost:/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                sbc.ReceiveEndpoint(host, "order-service", endpoint =>
                {
                    endpoint.Handler<SendEmailRequest>(async context =>
                    {
                        await Console.Out.WriteLineAsync($"Received: {context.Message.Id.ToString()}");
                        await context.RespondAsync<SendEmailResponse>(new
                        {
                            Id = 50
                        });
                    });

                    //endpoint.Consumer<ProcessRequest>();

                    //endpoint.Handler<SubmitEmailRequestContract>(async context =>
                    //{
                    //    await Console.Out.WriteLineAsync($"Received: {context.Message.Subject}");
                    //    EmailService emailService = new EmailService();
                    //    emailService.SendEmailThroughGmail(
                    //            context.Message.From
                    //          , context.Message.To
                    //          , context.Message.Subject
                    //          , context.Message.Body
                    //        );
                    //});
                });
            });

            await bus.StartAsync();
            try
            {
                Console.WriteLine("Working....");

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await bus.StopAsync();
            }
        }
    }
}