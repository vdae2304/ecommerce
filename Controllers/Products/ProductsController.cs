using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Controllers.Products.CreateProduct;
using Ecommerce.Controllers.Products.DeleteGalleryImage;
using Ecommerce.Controllers.Products.DeleteImage;
using Ecommerce.Controllers.Products.DeleteProduct;
using Ecommerce.Controllers.Products.EditProduct;
using Ecommerce.Controllers.Products.GetProduct;
using Ecommerce.Controllers.Products.SearchProducts;
using Ecommerce.Controllers.Products.UploadGalleryImage;
using Ecommerce.Controllers.Products.UploadImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Products
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Search products.
        /// </summary>
        /// <param name="filters">Search filters.</param>
        /// <response code="200">Ok. Return the list of products.</response>
        [ProducesResponseType(typeof(Response<SearchItems<Product>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchProductsRequest filters)
        {
            _logger.LogInformation("Search products");
            return await _mediator.Send(filters);
        }

        /// <summary>
        /// Get product details.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <response code="200">Ok. Return the product details.</response>
        /// <response code="404">Not Found. Product does not exist.</response>
        [ProducesResponseType(typeof(Response<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int productId)
        {
            _logger.LogInformation("Get details for product {productId}", productId);
            return await _mediator.Send(new GetProductRequest { ProductId = productId });
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="request">Product values.</param>
        /// <response code="200">Ok. Return the ID of the product created.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response<CreatedId>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            _logger.LogInformation("Create product {sku}", request.Sku);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Edit a product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <param name="request">Product values.</param>
        /// <response code="200">Ok. Update the product values.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("{productId}")]
        public async Task<IActionResult> Edit(int productId, [FromBody] EditProductRequest request)
        {
            request.ProductId = productId;
            _logger.LogInformation("Edit product {productId}", productId);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Delete a product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <response code="200">Ok. Delete the product.</response>
        /// <response code="404">Not Found. Product does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            _logger.LogInformation("Delete product {productId}", productId);
            return await _mediator.Send(new DeleteProductRequest {  ProductId = productId });
        }

        /// <summary>
        /// Upload main image for a product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <param name="imageFile">Image file.</param>
        /// <response code="200">Ok. Upload the image.</response>
        /// <response code="400">Bad request. Invalid file.</response>
        /// <response code="404">Not Found. Product does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [HttpPost("{productId}/image")]
        public async Task<IActionResult> UploadImage(int productId, IFormFile imageFile)
        {
            _logger.LogInformation("Upload main image for product {productId}", productId);
            return await _mediator.Send(new UploadImageRequest { ProductId = productId, ImageFile = imageFile });
        }

        /// <summary>
        /// Delete main image for a product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <response code="200">Ok. Delete the image.</response>
        /// <response code="404">Not Found. Product does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete("{productId}/image")]
        public async Task<IActionResult> DeleteImage(int productId)
        {
            _logger.LogInformation("Delete main image for product {productId}", productId);
            return await _mediator.Send(new DeleteImageRequest { ProductId = productId });
        }

        /// <summary>
        /// Upload a gallery image for a product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <param name="imageFile">Image file.</param>
        /// <response code="200">Ok. Upload the image.</response>
        /// <response code="400">Bad request. Invalid file.</response>
        /// <response code="404">Not Found. Product does not exist.</response>
        [ProducesResponseType(typeof(Response<CreatedId>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [HttpPost("{productId}/gallery")]
        public async Task<IActionResult> UploadGalleryImage(int productId, IFormFile imageFile)
        {
            _logger.LogInformation("Upload gallery image for product {productId}", productId);
            return await _mediator.Send(new UploadGalleryImageRequest { ProductId = productId, ImageFile = imageFile });
        }

        /// <summary>
        /// Delete a gallery image for a product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <param name="imageId">Image ID.</param>
        /// <response code="200">Ok. Delete the image.</response>
        /// <response code="404">Not Found. Product or image does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete("{productId}/gallery/{imageId}")]
        public async Task<IActionResult> DeleteGalleryImage(int productId, int imageId)
        {
            _logger.LogInformation("Delete gallery image {imageId} for product {productId}", imageId, productId);
            return await _mediator.Send(new DeleteGalleryImageRequest { ProductId = productId, ImageId = imageId });
        }
    }
}
