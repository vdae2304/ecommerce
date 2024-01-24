using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Categories.GetCategory
{
    public record GetCategoryRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
