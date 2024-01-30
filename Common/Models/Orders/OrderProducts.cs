using Ecommerce.Common.Models.Schema;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Orders
{
    /// <summary>
    /// Relationship between orders and products.
    /// </summary>
    public class OrderProducts
    {
        /// <summary>
        /// Order ID.
        /// </summary>
        /// <example>183721</example>
        [JsonIgnore]
        public int OrderId { get; set; }

        /// <summary>
        /// Product ID.
        /// </summary>
        /// <example>1037801</example>
        public int ProductId { get; set; }

        /// <summary>
        /// Product item.
        /// </summary>
        public virtual Product Product { get; set; } = new Product();

        /// <summary>
        /// Unit price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Product quantity.
        /// </summary>
        /// <example>10</example>
        public int Quantity { get; set; }

        /// <summary>
        /// Subtotal for this item.
        /// </summary>
        /// <example>80.00</example>
        public decimal Subtotal => Price * Quantity;
    }
}
