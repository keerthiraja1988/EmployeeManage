using CrossCutting.EmailService;
using MassTransit;
using MessageContracts;
using System;
using System.Threading.Tasks;

namespace SendEmailService
{
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

            // EmailService emailService = new EmailService();
            _iEmailService.SendEmailThroughGmail(
                                        context.Message.From
                                        , context.Message.To
                                        , context.Message.Subject
                                        , context.Message.Body
                                    );
            return Task.CompletedTask;
        }
    }
}