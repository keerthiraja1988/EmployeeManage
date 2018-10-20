using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.EmailService
{
    public class EmailService : IEmailService
    {
        public bool SendEmailThroughGmail(string From, string To, string Subject, string Body)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("keerthiraja1988@gmail.com", "K");
            client.EnableSsl = true;

            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("keerthiraja1988@gmail.com", "Keerthi Raja");
            mail.To.Add(new MailAddress(To));

            string htmlString = @"<html>
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
        bool SendEmailThroughGmail(string Sender, string Receivers, string Subject, string Body);
    }
}