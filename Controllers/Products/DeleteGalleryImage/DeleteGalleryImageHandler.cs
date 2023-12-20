﻿using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.DeleteGalleryImage
{
    public record DeleteGalleryImageRequest : IRequest<IActionResult>
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

    public class DeleteGalleryImageHandler : IRequestHandler<DeleteGalleryImageRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<DeleteGalleryImageHandler> _logger;

        public DeleteGalleryImageHandler(ApplicationDbContext context, IFileRepository fileRepository,
            ILogger<DeleteGalleryImageHandler> logger)
        {
            _context = context;
            _fileRepository = fileRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteGalleryImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _context.Products
                    .Include(x => x.GalleryImages)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                MediaImage image = product.GalleryImages
                    .FirstOrDefault(x => x.Id == request.ImageId)
                    ?? throw new NotFoundException($"Image {request.ImageId} does not exist");

                await _fileRepository.DeleteFileAsync(image.FileId);
                _context.MediaImages.Remove(image);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
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
