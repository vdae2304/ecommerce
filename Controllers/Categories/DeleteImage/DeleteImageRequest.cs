using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Categories.DeleteImage
{
    public record DeleteImageRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
