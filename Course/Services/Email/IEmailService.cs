using System.Net.Mail;

namespace Course.Services.Email
{
    public interface IEmailService
    {
        void Send(MailMessage mailMessage);
    }
}
