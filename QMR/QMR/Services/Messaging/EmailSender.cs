using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;

namespace QMR.Services.Messaging
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly EmailOptions _options;

        public EmailSender(ILoggerFactory loggerFactory, IOptions<EmailOptions> options)
        {
            _logger = loggerFactory.CreateLogger<EmailSender>();
            _options = options.Value;
        }

        public void SendEmail(string email, string subject, string message, bool html = false)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_options.AdminEmail);
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = html;

            using (var client = new SmtpClient(_options.MailServer, _options.MailPort))
            {
                client.Credentials = new System.Net.NetworkCredential(_options.MailUserId, _options.MailPassword);
                client.Send(mailMessage);
            }
        }

        public void SendException(Exception ex, string subject = " QMR Error")
        {
            string msg = "<pre>" + ex.ToString() + "</pre>";
            SendEmail(_options.SupportEmail, subject, msg, true);
        }
    }
}
