using System;

namespace QMR.Services.Messaging
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string message, bool html);

        void SendException(Exception ex, string subject);
    }
}
