using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Products.UploadImage
{
    public record UploadImageForm : IRequest<ActionResult>
    {
        /// <summary>
        /// Image file.
        /// </summary>
        [Required]
        public IFormFile ImageFile { get; set; }
    }

    public record UploadImageRequest : UploadImageForm
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
    }

    public class UploadImageHandler : IRequestHandler<UploadImageRequest, ActionResult>
    {
        private readonly IGenericRepository<Product> _products;
        private readonly IFileHandler _fileHandler;
        private readonly ILogger<UploadImageHandler> _logger;

        public UploadImageHandler(IGenericRepository<Product> products, IFileHandler fileHandler,
            ILogger<UploadImageHandler> logger)
        {
            _products = products;
            _fileHandler = fileHandler;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(UploadImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UploadImageValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                Product product = await _products.FindByIdAsync(request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                var image = System.Drawing.Image.FromStream(request.ImageFile.OpenReadStream());
                string fileId = _fileHandler.UploadFile(request.ImageFile);

                product.Thumbnail = new Image
                {
                    FileId = fileId,
                    Url = _fileHandler.GetFileUrl(fileId),
                    Width = image.Width,
                    Height = image.Height
                };

                await _products.UpdateAsync(product, cancellationToken);

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = $"Image uploaded with id {product.ThumbnailId}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in uploading main image for product {productId}", request.ProductId);
                throw;
            }
        }
    }
}
