using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace Course.Services.Email
{
    public class GmailService(IConfiguration configuration) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;

        public void Send(MailMessage mailMessage)
        {
            var smtp = _configuration.GetSection("smtp");
            String? host = smtp.GetSection("host").Value;
            String? email = smtp.GetSection("email").Value;
            String? password = smtp.GetSection("password").Value;
            Boolean ssl = smtp.GetValue<Boolean>("ssl");
            Int32 port = smtp.GetValue<Int32>("port");
            if (host == null || email == null || password == null)
            {
                throw new Exception("Config error");
            }
            using SmtpClient smtpClient = new()
            {
                Host = host,
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(email, password)
            };
            mailMessage.From = new MailAddress(email);
            if (String.IsNullOrEmpty(mailMessage.Subject))
            {
                mailMessage.Subject = "ASP site notification";
            }
            smtpClient.Send(mailMessage);
        }
    }
}
/*  // базові можливості
smtpClient.Send(
    from: email,
    recipients: "denniksam@gmail.com",
    subject: "ASP-212 site message",
    body: "Hello from the site!");
*/