using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Categories.CreateCategory
{
    public record CreateCategoryRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// ID of the parent category, if any.
        /// Default is to set as root category.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description.
        /// </summary>
        [JsonRequired]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ID of the products to assign to the category.
        /// </summary>
        public IEnumerable<int> ProductIds { get; set; } = new List<int>();

        /// <summary>
        /// Whether the category is enabled or not.
        /// Default is true.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
