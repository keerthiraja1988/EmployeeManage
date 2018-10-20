using CrossCutting.EmailService;
using CrossCutting.Logging;
using MassTransit;
using MessageContracts;
using System;
using System.Threading.Tasks;

namespace SendEmail
{
    [NLogging]
    internal class ProcessRequest : IConsumer<SubmitEmailRequestContract>
    {
        private readonly IEmailService _iEmailService;

        public ProcessRequest(IEmailService iEmailService)
        {
            _iEmailService = iEmailService;
        }

        public Task Consume(ConsumeContext<SubmitEmailRequestContract> context)
        {
            Console.WriteLine($"Sending Email To  {context.Message.To} { context.Message.SentOn.ToString()}");

            EmailService emailService = new EmailService();
            emailService.SendEmailThroughGmail(
                                        context.Message.From
                                        , context.Message.To
                                        , context.Message.Subject
                                        , context.Message.Body
                                    );
            return Task.CompletedTask;
        }
    }
}