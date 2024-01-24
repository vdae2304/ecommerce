using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Products.DeleteProduct
{
    public record DeleteProductRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
    }
}
