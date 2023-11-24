using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.DeleteGalleryImage
{
    public record DeleteGalleryImageRequest : IRequest<ActionResult>
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

    public class DeleteGalleryImageHandler : IRequestHandler<DeleteGalleryImageRequest, ActionResult>
    {
        private readonly IGenericRepository<Product> _products;
        private readonly IGenericRepository<Image> _images;
        private readonly IFileHandler _fileHandler;
        private readonly ILogger<DeleteGalleryImageHandler> _logger;

        public DeleteGalleryImageHandler(IGenericRepository<Product> products, IGenericRepository<Image> images,
            IFileHandler fileHandler, ILogger<DeleteGalleryImageHandler> logger)
        {
            _products = products;
            _images = images;
            _fileHandler = fileHandler;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(DeleteGalleryImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _products.AsQueryable()
                    .Include(x => x.GalleryImages)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                Image image = product.GalleryImages
                    .FirstOrDefault(x => x.Id == request.ImageId)
                    ?? throw new NotFoundException($"Image {request.ImageId} does not exist");

                _fileHandler.DeleteFile(image.FileId);
                await _images.DeleteAsync(image, cancellationToken);

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting gallery image {imageId} for product {productId}", request.ImageId, request.ProductId);
                throw;
            }
        }
    }
}
