using Microsoft.Extensions.Configuration;
using MimeKit;

using MailKit.Net.Smtp;

using LibraryManagementSystem.BLL.ServicesContracts;

namespace LibraryManagementSystem.BLL.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["MailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 465, true);
            await smtp.AuthenticateAsync("ahmedghona054@gmail.com", "klcy iehy ihoi broj");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
