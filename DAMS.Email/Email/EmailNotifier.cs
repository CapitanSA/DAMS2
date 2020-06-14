using System.Net;
using DAMS.EventReminder;
using DAMS.EventReminder.Notifier;
using System.Net.Mail;
using System.Configuration;
using DAMS.Helpers;
using System;

namespace DAMS.NotificationSystems.All.Email
{
    public class EmailNotifier : INotifier
    {
        public NotificationResult Notify(NotificationInfo eventInfo)
        {
            var regexEmail = new RegexValidator();
            var smtpClient = PrepareSmtpClient();
            var email = FormEmail(eventInfo);

            NotificationResult result = new NotificationResult();
            try
            {
                if (regexEmail.Email(eventInfo.EmailRecipient) == true)
                {
                    smtpClient.Send(email);
                    result.IsSuccess = true;
                }
            }
            catch(Exception ex) 
            {
                if (regexEmail.Email(eventInfo.EmailRecipient) == false)
                {
                    result.Details = ex.Message;
                }
            }
            return result; 
        }

        private MailMessage FormEmail(NotificationInfo eventInfo)
        {
            string mailFrom = ConfigurationManager.AppSettings.Get("FromMailAddress");
            string from = ConfigurationManager.AppSettings.Get("NameAdmin");
            MailAddress fromMailAddress = new MailAddress(mailFrom, from);
            MailAddress toAddress = new MailAddress(eventInfo.EmailRecipient);
            MailMessage mailMessage = new MailMessage(fromMailAddress, toAddress);
            mailMessage.Subject = eventInfo.EventName;
            mailMessage.Body = eventInfo.EventName + eventInfo.EventDate; 
            mailMessage.IsBodyHtml = true;
            return mailMessage;
        }

        private SmtpClient PrepareSmtpClient()
        {
            string smtpHost = ConfigurationManager.AppSettings.Get("Host_smtp");
            string login = ConfigurationManager.AppSettings.Get("Login");
            string password = ConfigurationManager.AppSettings.Get("Password");
            var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(login, password),
            };
            return smtp;
        }
    }
}