using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Categories.UploadImage
{
    public record UploadImageRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
        /// <summary>
        /// Image file.
        /// </summary>
        [Required]
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

                Category category = await _context.Categories
                    .Include(x => x.Thumbnail)
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException($"Category {request.CategoryId} does not exist");

                var image = await Image.LoadAsync(request.ImageFile.OpenReadStream(), cancellationToken);

                string fileId = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                await _fileRepository.UploadFileAsync(request.ImageFile.OpenReadStream(), fileId);

                MediaImage? oldThumbnail = category.Thumbnail;
                category.Thumbnail = new MediaImage
                {
                    FileId = fileId,
                    Url = _fileRepository.GetFileUrl(fileId),
                    Width = image.Width,
                    Height = image.Height
                };

                if (oldThumbnail != null)
                {
                    await _fileRepository.DeleteFileAsync(oldThumbnail.FileId);
                    _context.MediaImages.Remove(oldThumbnail);
                }

                _context.Categories.Update(category);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = $"Image uploaded with id {category.Thumbnail.Id}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in uploading main image for category {categoryId}", request.CategoryId);
                throw;
            }
        }
    }
}
