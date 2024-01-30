using Ecommerce.Common.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Ecommerce.Infrastructure.Implementation
{
    /// <inheritdoc/>
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _sender;
        private readonly string _password;

        public EmailNotificationService(IConfiguration configuration)
        {
            _host = configuration["EmailService:Host"] ?? string.Empty;
            _port = int.Parse(configuration["EmailService:Port"] ?? string.Empty);
            _sender = configuration["EmailService:Sender"] ?? string.Empty;
            _password = configuration["EmailService:Password"] ?? string.Empty;
        }

        /// <inheritdoc/>
        public async Task SendEmailAsync(EmailMessage message)
        {
            var smtpClient = new SmtpClient
            {
                Host = _host,
                Port = _port,
                Credentials = new NetworkCredential(_sender, _password),
                EnableSsl = true,
                Timeout = 60000
            };

            var mailMessage = new MailMessage(_sender, message.Recipient)
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            };
            foreach (Attachment attachment in message.Attachments)
            {
                mailMessage.Attachments.Add(attachment);
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
