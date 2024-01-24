using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Products.DeleteImage
{
    public record DeleteImageRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
    }
}
