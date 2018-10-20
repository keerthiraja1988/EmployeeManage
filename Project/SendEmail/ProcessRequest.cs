using MassTransit;
using MessageContracts;
using System;
using System.Threading.Tasks;

namespace SendEmail
{
    internal class ProcessRequest : IConsumer<SendEmailRequest>
    {
        public Task Consume(ConsumeContext<SendEmailRequest> context)
        {
            Console.WriteLine($"Adding user {context.Message.Id}");
            return Task.CompletedTask;
        }
    }
}