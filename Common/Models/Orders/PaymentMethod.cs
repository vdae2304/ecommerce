using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Orders
{
    /// <summary>
    /// Payment method model.
    /// </summary>
    public class PaymentMethod : IEntity
    {
        /// <summary>
        /// Payment method ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User ID linked to the payment method.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Card owner.
        /// </summary>
        /// <example>John Doe</example>
        public string CardOwner { get; set; } = string.Empty;

        /// <summary>
        /// Card number.
        /// </summary>
        /// <example>8169271974913941</example>
        [JsonIgnore]
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        /// Last 4 digits.
        /// </summary>
        /// <example>3941</example>
        public string Last4 => CardNumber[^4..];

        /// <summary>
        /// Card verification value.
        /// </summary>
        /// <example>719</example>
        [JsonIgnore]
        public string CVV { get; set; } = string.Empty;

        /// <summary>
        /// Month of expiration.
        /// </summary>
        /// <example>08</example>
        public string ExpiryMonth { get; set; } = string.Empty;

        /// <summary>
        /// Year of expiration.
        /// </summary>
        /// <example>2024</example>
        public string ExpiryYear { get; set; } = string.Empty;

        /// <summary>
        /// Billing address ID.
        /// </summary>
        [JsonIgnore]
        public int BillingAddressId { get; set; }

        /// <summary>
        /// Billing address.
        /// </summary>
        public Address BillingAddress { get; set; } = new Address();

        /// <summary>
        /// Datetime of creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Datetime of last modification.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
