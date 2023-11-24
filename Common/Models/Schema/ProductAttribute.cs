using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Product attribute.
    /// </summary>
    public class ProductAttribute : IEntity
    {
        /// <summary>
        /// Attribute ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product ID the attribute belongs to.
        /// </summary>
        [JsonIgnore]
        public int ProductId { get; set; }

        /// <summary>
        /// Attribute name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Attribute value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Datetime of creation.
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Datetime of last modification.
        /// </summary>
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
