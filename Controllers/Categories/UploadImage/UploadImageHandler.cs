﻿using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace Ecommerce.Controllers.Categories.UploadImage
{
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
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                Category category = await _context.Categories
                    .Include(x => x.Thumbnail)
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException();

                var image = await Image.LoadAsync(request.ImageFile.OpenReadStream(), cancellationToken);

                string filename = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                string mimeType = request.ImageFile.ContentType;
                await _fileRepository.UploadFileAsync(request.ImageFile.OpenReadStream(), filename);

                MediaImage? oldThumbnail = category.Thumbnail;
                category.Thumbnail = new MediaImage
                {
                    Url = _fileRepository.GetFileUrl(filename),
                    Filename = filename,
                    MimeType = mimeType,
                    Width = image.Width,
                    Height = image.Height
                };

                if (oldThumbnail != null)
                {
                    await _fileRepository.DeleteFileAsync(oldThumbnail.Filename);
                    _context.MediaImages.Remove(oldThumbnail);
                }

                _context.Categories.Update(category);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
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
