using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Orders
{
    /// <summary>
    /// Shipping address model.
    /// </summary>
    public class ShippingAddress : IEntity
    {
        /// <summary>
        /// Shipping address ID.
        /// </summary>
        /// <example>71928</example>
        public int Id { get; set; }

        /// <summary>
        /// User ID linked to the shipping address.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Contact name.
        /// </summary>
        /// <example>John Doe</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number.
        /// </summary>
        /// <example>5523917391</example>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Street.
        /// </summary>
        /// <example>Av. Cuauhtemoc 19</example>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// City.
        /// </summary>
        /// <example>Mexico City</example>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State/province code.
        /// </summary>
        /// <example>CDMX</example>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Postal code (zip code).
        /// </summary>
        /// <example>06600</example>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Additional comments.
        /// </summary>
        public string? Comments { get; set; }

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
