using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.DeleteImage
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
                Category category = await _context.Categories
                    .Include(x => x.Thumbnail)
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException();

                if (category.Thumbnail != null)
                {
                    await _fileRepository.DeleteFileAsync(category.Thumbnail.Filename);
                    _context.MediaImages.Remove(category.Thumbnail);
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
                _logger.LogError(ex, "Error in deleting main image for category {categoryId}", request.CategoryId);
                throw;
            }
        }
    }
}
