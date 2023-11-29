using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Categories.DeleteCategory
{
    public record DeleteCategoryRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
    }

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<DeleteCategoryHandler> _logger;

        public DeleteCategoryHandler(ApplicationDbContext context, IFileRepository fileRepository,
            ILogger<DeleteCategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = await _context.Categories
                    .Include(x => x.ThumbnailId)
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException($"Category {request.CategoryId} does not exist");

                if (category.Thumbnail != null)
                {
                    await _fileRepository.DeleteFileAsync(category.Thumbnail.FileId);
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
