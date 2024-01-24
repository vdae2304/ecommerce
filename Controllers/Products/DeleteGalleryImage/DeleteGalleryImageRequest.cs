using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Products.DeleteGalleryImage
{
    public record DeleteGalleryImageRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Image ID.
        /// </summary>
        public int ImageId { get; set; }
    }
}
