using MimeKit;
using MailKit.Net.Smtp;
using TK_UR_BOOK.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TK_UR_BOOK.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailForm = _configuration["EmailSettings:Email"];
            var emailPassword = _configuration["EmailSettings:Password"];
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailForm));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            //Smtp server configuration for Gmail
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailForm, emailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
