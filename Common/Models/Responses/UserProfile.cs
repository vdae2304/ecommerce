namespace Ecommerce.Common.Models.Responses
{
    /// <summary>
    /// User profile response.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// User ID.
        /// </summary>
        /// <example>129385</example>
        public int Id { get; set; }
        /// <summary>
        /// Username.
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }
}
