using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(ApplicationDbContext context, IFileRepository fileRepository,
            ILogger<DeleteProductHandler> logger)
        {
            _context = context;
            _fileRepository = fileRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _context.Products
                    .Include(x => x.Thumbnail)
                    .Include(x => x.GalleryImages)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException();

                if (product.Thumbnail != null)
                {
                    await _fileRepository.DeleteFileAsync(product.Thumbnail.Filename);
                    _context.MediaImages.Remove(product.Thumbnail);
                }

                foreach (MediaImage galleryImage in product.GalleryImages)
                {
                    await _fileRepository.DeleteFileAsync(galleryImage.Filename);
                    _context.MediaImages.Remove(galleryImage);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting product {productId}", request.ProductId);
                throw;
            }
        }
    }
}
