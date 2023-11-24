using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.DeleteImage
{
    public record DeleteImageRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int CategoryId { get; set; }
    }

    public class DeleteImageHandler : IRequestHandler<DeleteImageRequest, ActionResult>
    {
        private readonly IGenericRepository<Category> _categories;
        private readonly IGenericRepository<Image> _images;
        private readonly IFileHandler _fileHandler;
        private readonly ILogger<DeleteImageHandler> _logger;

        public DeleteImageHandler(IGenericRepository<Category> categories, IGenericRepository<Image> images,
            IFileHandler fileHandler, ILogger<DeleteImageHandler> logger)
        {
            _categories = categories;
            _images = images;
            _fileHandler = fileHandler;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(DeleteImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = await _categories.AsQueryable()
                    .Include(x => x.Thumbnail)
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException($"Category {request.CategoryId} does not exist");

                if (category.Thumbnail != null)
                {
                    _fileHandler.DeleteFile(category.Thumbnail.FileId);
                    await _images.DeleteAsync(category.Thumbnail, cancellationToken);
                }

                return new OkObjectResult(new StatusResponse
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
