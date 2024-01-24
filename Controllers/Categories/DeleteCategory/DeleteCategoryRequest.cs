using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Categories.DeleteCategory
{
    public record DeleteCategoryRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
