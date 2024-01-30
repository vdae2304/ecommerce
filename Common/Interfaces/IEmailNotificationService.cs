namespace Ecommerce.Common.Interfaces
{
    /// <summary>
    /// Email notification service.
    /// </summary>
    public interface IEmailNotificationService
    {
        /// <summary>
        /// Send an email asynchronously.
        /// </summary>
        /// <param name="message">Email message to send.</param>
        /// <returns></returns>
        public Task SendEmailAsync(EmailMessage message);
    }
}
