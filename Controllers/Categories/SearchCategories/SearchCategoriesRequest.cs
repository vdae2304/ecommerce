using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Categories.SearchCategories
{
    public record SearchCategoriesRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Categories to search for.
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Search for categories that are a subcategory of this.
        /// Set to 0 to get root categories.
        /// </summary>
        public int? Parent { get; set; }

        /// <summary>
        /// If true, show only enabled categories.
        /// If false, show only disabled categories.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Number of items to skip at the beginning.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Maximum number of items to return.
        /// </summary>
        public int Limit { get; set; } = 100;
    }
}
