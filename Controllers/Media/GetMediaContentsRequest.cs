using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Media
{
    public record GetMediaContentsRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Filename.
        /// </summary>
        public string Filename { get; set; } = string.Empty;
    }
}
