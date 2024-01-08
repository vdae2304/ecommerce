using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace Ecommerce.Controllers.Products.UploadImage
{
    public record UploadImageRequest : IRequest<IActionResult>
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

    public class UploadImageHandler : IRequestHandler<UploadImageRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<UploadImageHandler> _logger;

        public UploadImageHandler(ApplicationDbContext context, IFileRepository fileRepository,
            ILogger<UploadImageHandler> logger)
        {
            _context = context;
            _fileRepository = fileRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(UploadImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UploadImageValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                Product product = await _context.Products
                    .Include(x => x.Thumbnail)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                var image = await Image.LoadAsync(request.ImageFile.OpenReadStream(), cancellationToken);

                string filename = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                string mimeType = request.ImageFile.ContentType;
                await _fileRepository.UploadFileAsync(request.ImageFile.OpenReadStream(), filename);

                MediaImage? oldThumbnail = product.Thumbnail;
                product.Thumbnail = new MediaImage
                {
                    Url = _fileRepository.GetFileUrl(filename),
                    Filename = filename,
                    MimeType = mimeType,
                    Width = image.Width,
                    Height = image.Height
                };

                if (oldThumbnail != null)
                {
                    await _fileRepository.DeleteFileAsync(filename);
                    _context.MediaImages.Remove(oldThumbnail);
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
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
