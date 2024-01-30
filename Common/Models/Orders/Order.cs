using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Orders
{
    /// <summary>
    /// Order model.
    /// </summary>
    public class Order : IEntity
    {
        /// <summary>
        /// Order ID.
        /// </summary>
        /// <example>183721</example>
        public int Id { get; set; }

        /// <summary>
        /// User ID who placed the order.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// List of products contained in the order.
        /// </summary>
        public virtual IEnumerable<OrderProducts> Items { get; set; } = new List<OrderProducts>();

        /// <summary>
        /// Subtotal.
        /// </summary>
        /// <example>109</example>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Formatted subtotal.
        /// </summary>
        /// <example>$109.00</example>
        public string FormattedSubtotal => $"{Subtotal:C}";

        /// <summary>
        /// Payment method ID.
        /// </summary>
        [JsonIgnore]
        public int? PaymentMethodId { get; set; }

        /// <summary>
        /// Payment method.
        /// </summary>
        public virtual PaymentMethod? PaymentMethod { get; set; }

        /// <summary>
        /// Payment status.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// Shipping address ID.
        /// </summary>
        [JsonIgnore]
        public int? ShippingAddressId { get; set; }

        /// <summary>
        /// Shipping address.
        /// </summary>
        public virtual Address? ShippingAddress { get; set; }

        /// <summary>
        /// Shipping status.
        /// </summary>
        public ShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// Shipping cost.
        /// </summary>
        /// <example>0.00</example>
        public decimal ShippingCost { get; set; }

        /// <summary>
        /// Formatted shipping cost.
        /// </summary>
        /// <example>$0.00</example>
        public string FormattedShippingCost => $"{ShippingCost:C}";

        /// <summary>
        /// Tracking number.
        /// </summary>
        public string? TrackingNumber { get; set; }

        /// <summary>
        /// Total to pay.
        /// </summary>
        /// <example>110.00</example>
        public decimal Total { get; set; }

        /// <summary>
        /// Formatted total.
        /// </summary>
        /// <example>$110.00</example>
        public string FormattedTotal => $"{Total:C}";
        
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
