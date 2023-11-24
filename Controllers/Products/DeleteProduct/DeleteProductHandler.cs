using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Products.DeleteProduct
{
    public record DeleteProductRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        [Required]
        public int ProductId { get; set; }
    }

    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, ActionResult>
    {
        private readonly IGenericRepository<Product> _products;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(IGenericRepository<Product> products, ILogger<DeleteProductHandler> logger)
        {
            _products = products;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _products.FindByIdAsync(request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                await _products.DeleteAsync(product, cancellationToken);

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting product {productId}", request.ProductId);
                throw;
            }
        }
    }
}
