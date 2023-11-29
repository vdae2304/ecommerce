using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Controllers.Categories.CreateCategory;
using Ecommerce.Controllers.Categories.DeleteCategory;
using Ecommerce.Controllers.Categories.DeleteImage;
using Ecommerce.Controllers.Categories.GetCategory;
using Ecommerce.Controllers.Categories.SearchCategories;
using Ecommerce.Controllers.Categories.UploadImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Categories
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Search categories.
        /// </summary>
        /// <param name="filters">Search filters.</param>
        /// <response code="200">Ok. Return the list of categories.</response>
        [ProducesResponseType(typeof(Response<SearchItems<Category>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] CategoryFilters filters)
        {
            _logger.LogInformation("Search categories");
            return await _mediator.Send(filters);
        }

        /// <summary>
        /// Get category details.
        /// </summary>
        /// <param name="categoryId">Category ID.</param>
        /// <response code="200">Ok. Return the category details.</response>
        /// <response code="404">Not Found. Category does not exist.</response>
        [ProducesResponseType(typeof(Response<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> Get(int categoryId)
        {
            _logger.LogInformation("Get details for category {categoryId}", categoryId);
            return await _mediator.Send(new GetCategoryRequest { CategoryId = categoryId });
        }

        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="request">Category values.</param>
        /// <response code="200">Ok. Return the ID of the category created.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryForm request)
        {
            _logger.LogInformation("Create category {name}", request.Name);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Delete a category.
        /// </summary>
        /// <param name="categoryId">Category ID.</param>
        /// <response code="200">Ok. Delete the category.</response>
        /// <response code="404">Not Found. Category does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            _logger.LogInformation("Delete category {categoryId}", categoryId);
            return await _mediator.Send(new DeleteCategoryRequest { CategoryId = categoryId });
        }

        /// <summary>
        /// Upload main image for a category.
        /// </summary>
        /// <param name="categoryId">Category ID.</param>
        /// <param name="imageFile">Image file.</param>
        /// <response code="200">Ok. Upload the image.</response>
        /// <response code="400">Bad request. Invalid file.</response>
        /// <response code="404">Not Found. Category does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Consumes("multipart/form-data")]
        [HttpPost("{categoryId}/image")]
        public async Task<IActionResult> UploadImage(int categoryId, IFormFile imageFile)
        {
            _logger.LogInformation("Upload main image for category {categoryId}", categoryId);
            return await _mediator.Send(new UploadImageRequest { CategoryId = categoryId, ImageFile = imageFile });
        }

        /// <summary>
        /// Delete main image for a category.
        /// </summary>
        /// <param name="categoryId">Category ID.</param>
        /// <response code="200">Ok. Delete the image.</response>
        /// <response code="404">Not Found. Category does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{categoryId}/image")]
        public async Task<IActionResult> DeleteImage(int categoryId)
        {
            _logger.LogInformation("Delete main image for category {categoryId}", categoryId);
            return await _mediator.Send(new DeleteImageRequest { CategoryId = categoryId });
        }
    }
}
