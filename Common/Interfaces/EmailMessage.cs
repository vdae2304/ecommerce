using System.Net.Mail;

namespace Ecommerce.Common.Interfaces
{
    /// <summary>
    /// Email message.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Recipient.
        /// </summary>
        public string Recipient { get; set; } = string.Empty;
        /// <summary>
        /// Subject.
        /// </summary>
        public string Subject { get; set; } = string.Empty;
        /// <summary>
        /// Message body.
        /// </summary>
        public string Body { get; set; } = string.Empty;
        /// <summary>
        /// Whether the message body is in html.
        /// </summary>
        public bool IsBodyHtml { get; set; } = false;
        /// <summary>
        /// List of attachments.
        /// </summary>
        public IEnumerable<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
