using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Categories.UploadImage
{
    public record UploadImageRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Image file.
        /// </summary>
        public IFormFile ImageFile { get; set; }
    }
}
