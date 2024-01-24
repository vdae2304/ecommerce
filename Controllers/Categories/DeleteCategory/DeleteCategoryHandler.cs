using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<DeleteCategoryHandler> _logger;

        public DeleteCategoryHandler(ApplicationDbContext context, IFileRepository fileRepository,
            ILogger<DeleteCategoryHandler> logger)
        {
            _context = context;
            _fileRepository = fileRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
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
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting category {categoryId}", request.CategoryId);
                throw;
            }
        }
    }
}
