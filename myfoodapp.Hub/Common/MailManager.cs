using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace myfoodapp.Hub.Common
{
    public class MailManager
    {
        public static void SendMail(string receiver, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(receiver));
            message.From = new MailAddress(ConfigurationManager.AppSettings["mailSenderAccountAlias"]);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = ConfigurationManager.AppSettings["mailSenderAccount"],
                    Password = ConfigurationManager.AppSettings["mailSenderAccountPassword"]
                };
                smtp.Credentials = credential;
                smtp.Host = ConfigurationManager.AppSettings["mailSenderAccountHost"];
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["mailSenderAccountPort"]);
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
        }
    }
}