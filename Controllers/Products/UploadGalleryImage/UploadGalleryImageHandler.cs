using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Products.UploadGalleryImage
{
    public record UploadGalleryImageRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Image file.
        /// </summary>
        [Required]
        public IFormFile ImageFile { get; set; }
    }

    public class UploadGalleryImageHandler : IRequestHandler<UploadGalleryImageRequest, ActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<UploadGalleryImageHandler> _logger;

        public UploadGalleryImageHandler(ApplicationDbContext context, IFileRepository fileRepository,
            ILogger<UploadGalleryImageHandler> logger)
        {
            _context = context;
            _fileRepository = fileRepository;
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

                Product product = await _context.Products
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                var image = await Image.LoadAsync(request.ImageFile.OpenReadStream(), cancellationToken);

                string fileId = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                await _fileRepository.UploadFileAsync(request.ImageFile.OpenReadStream(), fileId);

                product.GalleryImages.Add(new MediaImage
                {
                    FileId = fileId,
                    Url = _fileRepository.GetFileUrl(fileId),
                    Width = image.Width,
                    Height = image.Height
                });

                _context.Products.Update(product);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new CreatedId { Id = product.GalleryImages.Last().Id }
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
