using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Products.UploadGalleryImage
{
    public record UploadGalleryImageRequest : IRequest<IActionResult>
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
