using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.DeleteImage
{
    public class DeleteImageHandler : IRequestHandler<DeleteImageRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<DeleteImageHandler> _logger;

        public DeleteImageHandler(ApplicationDbContext context, IFileRepository fileRepository,
            ILogger<DeleteImageHandler> logger)
        {
            _context = context;
            _fileRepository = fileRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _context.Products
                    .Include(x => x.Thumbnail)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException();

                if (product.Thumbnail != null)
                {
                    await _fileRepository.DeleteFileAsync(product.Thumbnail.Filename);
                    _context.MediaImages.Remove(product.Thumbnail);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting main image for product {productId}", request.ProductId);
                throw;
            }
        }
    }
}
