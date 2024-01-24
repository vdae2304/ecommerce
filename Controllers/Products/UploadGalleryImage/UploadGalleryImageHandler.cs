using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace Ecommerce.Controllers.Products.UploadGalleryImage
{
    public class UploadGalleryImageHandler : IRequestHandler<UploadGalleryImageRequest, IActionResult>
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

        public async Task<IActionResult> Handle(UploadGalleryImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UploadGalleryImageValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                Product product = await _context.Products
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException();

                var image = await Image.LoadAsync(request.ImageFile.OpenReadStream(), cancellationToken);

                string filename = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                string mimeType = request.ImageFile.ContentType;
                await _fileRepository.UploadFileAsync(request.ImageFile.OpenReadStream(), filename);

                product.GalleryImages.Add(new MediaImage
                {
                    Url = _fileRepository.GetFileUrl(filename),
                    Filename = filename,
                    MimeType = mimeType,
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
