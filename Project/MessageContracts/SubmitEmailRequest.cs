using System;

namespace MessageContracts
{
    // Message Definition
    public class MyMessage
    {
        public string Value { get; set; }
    }

    public class SubmitEmailRequestContract
    {
        public string From { get; set; }
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
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