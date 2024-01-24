using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Products.UploadImage
{
    public record UploadImageRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Image file.
        /// </summary>
        public IFormFile ImageFile { get; set; }
    }
}
