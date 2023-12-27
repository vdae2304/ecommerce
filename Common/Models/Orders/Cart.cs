using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Schema;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Orders
{
    /// <summary>
    /// Relationship between carts and products.
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// User ID who owns the cart.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// User who owns the cart.
        /// </summary>
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; } = new ApplicationUser();

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
        /// Product quantity.
        /// </summary>
        /// <example>10</example>
        public int Quantity { get; set; }

        /// <summary>
        /// Subtotal for this item, i.e. product price x quantity.
        /// </summary>
        /// <example>80.00</example>
        public decimal Subtotal => Product.Price * Quantity;
    }
}
