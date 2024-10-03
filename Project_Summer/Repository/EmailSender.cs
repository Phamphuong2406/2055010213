using System.Net.Mail;
using System.Net;

namespace Project_Summer.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("phamtphuong2406@gmail.com", "wmokyacdkcabynwq")
            };

            return client.SendMailAsync(
                new MailMessage(from: "phamtphuong2406@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
