using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Products.UploadGalleryImage
{
    public record UploadGalleryImageForm : IRequest<ActionResult>
    {
        /// <summary>
        /// Image file.
        /// </summary>
        [Required]
        public IFormFile ImageFile { get; set; }
    }

    public record UploadGalleryImageRequest : UploadGalleryImageForm
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
    }

    public class UploadGalleryImageHandler : IRequestHandler<UploadGalleryImageRequest, ActionResult>
    {
        private readonly IGenericRepository<Product> _products;
        private readonly IGenericRepository<Image> _images;
        private readonly IFileHandler _fileHandler;
        private readonly ILogger<UploadGalleryImageHandler> _logger;

        public UploadGalleryImageHandler(IGenericRepository<Product> products, IGenericRepository<Image> images,
            IFileHandler fileHandler, ILogger<UploadGalleryImageHandler> logger)
        {
            _products = products;
            _images = images;
            _fileHandler = fileHandler;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(UploadGalleryImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UploadGalleryImageValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                Product product = await _products.FindByIdAsync(request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                var image = System.Drawing.Image.FromStream(request.ImageFile.OpenReadStream());
                string fileId = _fileHandler.UploadFile(request.ImageFile);

                product.GalleryImages.Add(new Image
                {
                    FileId = fileId,
                    Url = _fileHandler.GetFileUrl(fileId),
                    Width = image.Width,
                    Height = image.Height
                });

                await _products.UpdateAsync(product, cancellationToken);

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in uploading gallery image for product {productId}", request.ProductId);
                throw;
            }
        }
    }
}
