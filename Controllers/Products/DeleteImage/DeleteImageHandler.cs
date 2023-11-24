using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.DeleteImage
{
    public record DeleteImageRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
    }

    public class DeleteImageHandler : IRequestHandler<DeleteImageRequest, ActionResult>
    {
        private readonly IGenericRepository<Product> _products;
        private readonly IGenericRepository<Image> _images;
        private readonly IFileHandler _fileHandler;
        private readonly ILogger<DeleteImageHandler> _logger;

        public DeleteImageHandler(IGenericRepository<Product> products, IGenericRepository<Image> images,
            IFileHandler fileHandler, ILogger<DeleteImageHandler> logger)
        {
            _products = products;
            _images = images;
            _fileHandler = fileHandler;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(DeleteImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _products.AsQueryable()
                    .Include(x => x.Thumbnail)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                if (product.Thumbnail != null)
                {
                    _fileHandler.DeleteFile(product.Thumbnail.FileId);
                    await _images.DeleteAsync(product.Thumbnail, cancellationToken);
                }

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting main image for product {productId}", request.ProductId);
                throw;
            }
        }
    }
}
