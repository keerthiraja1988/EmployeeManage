using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CrossCutting.EmailService
{
    public class EmailService : IEmailService
    {
       public bool SendEmailThroughGMail(string sender, string receivers,
                string subject, string body)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("keerthiraja1988@gmail.com", ""),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };


            MailMessage mail = new MailMessage {From = new MailAddress("keerthiraja1988@gmail.com", "Sender")};
            mail.To.Add(new MailAddress("keerthiraja1988@gmail.com"));

            const string htmlString = @"<html>
                      <body>
                      <p>Dear Ms. Susan,</p>
                      <p>Thank you for your letter of yesterday inviting me to come for an interview on Friday afternoon, 5th July, at 2:30.
                              I shall be happy to be there as requested and will bring my diploma and other papers with me.</p>
                      <p>Sincerely,<br>-Jack</br></p>
                      </body>
                      </html>
                     ";
            mail.Body = htmlString;

            mail.Subject = "TEST";
            mail.IsBodyHtml = true;

            client.Send(mail);

            return true;
        }
    }

    public interface IEmailService
    {
        bool SendEmailThroughGMail(string sender, string receivers, string subject, string body);
    }
}
