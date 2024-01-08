using Ecommerce.Common.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Ecommerce.Infrastructure.Implementation
{
    /// <summary>
    /// Email notification service options.
    /// </summary>
    public class EmailNotificationOptions
    {
        /// <summary>
        /// Host.
        /// </summary>
        public string Host { get; set; } = string.Empty;
        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Email sender.
        /// </summary>
        public string Sender { get; set; } = string.Empty;
        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <inheritdoc/>
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly EmailNotificationOptions _options;

        public EmailNotificationService(IOptions<EmailNotificationOptions> options)
        {
            _options = options.Value;
        }

        /// <inheritdoc/>
        public async Task SendEmailAsync(EmailMessage message)
        {
            var smtpClient = new SmtpClient
            {
                Host = _options.Host,
                Port = _options.Port,
                Credentials = new NetworkCredential(_options.Sender, _options.Password),
                EnableSsl = true,
                Timeout = 60000
            };

            var mailMessage = new MailMessage(_options.Sender, message.Recipient)
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
