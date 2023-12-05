using Ecommerce.Common.Models.IAM;

namespace Ecommerce.Common.Models.Responses
{
    /// <summary>
    /// Authentication response.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Token type.
        /// </summary>
        /// <example>Bearer</example>
        public string TokenType { get; set; } = string.Empty;
        /// <summary>
        /// Token value.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// Datetime of expiration.
        /// </summary>
        public DateTime Expires { get; set; }
        /// <summary>
        /// User information.
        /// </summary>
        public UserProfile User { get; set; } = new UserProfile();
    }
}
