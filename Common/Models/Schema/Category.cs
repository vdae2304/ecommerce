using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Category.
    /// </summary>
    public class Category : IEntity
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the parent category, if any.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Thumbnail ID.
        /// </summary>
        [JsonIgnore]
        public int? ThumbnailId { get; set; }

        /// <summary>
        /// Thumbnail.
        /// </summary>
        public virtual MediaImage? Thumbnail { get; set; }
        
        /// <summary>
        /// Children categories.
        /// </summary>
        public virtual ICollection<Category> Subcategories { get; set; } = new List<Category>();

        /// <summary>
        /// List of products assigned to the category.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Whether the category is enabled or not.
        /// </summary>
        public bool? Enabled { get; set; }

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
