using System;

namespace MessageContracts
{
    // Message Definition
    public class MyMessage
    {
        public string Value { get; set; }
    }

    public interface SubmitEmailRequestContract
    {
        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Body { get; set; }

        DateTime SentOn { get; set; }
    }

    public interface SendEmailRequest
    {
        long Id { get; set; }
    }

    public interface SendEmailResponse
    {
        long Id { get; set; }
    }
}