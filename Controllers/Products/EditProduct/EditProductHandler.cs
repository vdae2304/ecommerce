using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Controllers.Products.CreateProduct;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Products.EditProduct
{
    public record EditProductForm : IRequest<IActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        [JsonIgnore]
        public int ProductId { get; set; }

        /// <summary>
        /// An unique identifier for the product.
        /// </summary>
        [JsonRequired]
        public string Sku { get; set; } = string.Empty;

        /// <summary>
        /// Product name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product description.
        /// </summary>
        [JsonRequired]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Product price.
        /// </summary>
        [JsonRequired]
        public decimal Price { get; set; }

        /// <summary>
        /// If not null, display a price to compare to.
        /// </summary>
        public decimal? CrossedOutPrice { get; set; }

        /// <summary>
        /// ID of the categories the product is assigned to.
        /// </summary>
        public IEnumerable<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// Product attributes.
        /// </summary>
        public List<CreateAttributeForm> Attributes { get; set; } = new List<CreateAttributeForm>();

        /// <summary>
        /// (Optional) Product length.
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// (Optional) Product width.
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// (Optional) Product height.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// (Optional) Dimension units.
        /// </summary>
        public DimensionUnits? DimensionUnits { get; set; }

        /// <summary>
        /// (Optional) Product weight.
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// (Optional) Weight units.
        /// </summary>
        public WeightUnits? WeightUnits { get; set; }

        /// <summary>
        /// Minimum allowed purchase quantity. Default is 1.
        /// </summary>
        public int MinPurchaseQuantity { get; set; } = 1;

        /// <summary>
        /// Maximum allowed purchase quantity. Default is 1.
        /// </summary>
        public int MaxPurchaseQuantity { get; set; } = 1;

        /// <summary>
        /// Available number of products in stock.
        /// Default is 0.
        /// </summary>
        public int InStock { get; set; } = 0;

        /// <summary>
        /// Whether the product has unlimited stock.
        /// Default is false.
        /// </summary>
        public bool Unlimited { get; set; } = false;

        /// <summary>
        /// Whether the product is enabled or not.
        /// Default is true.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }

    public class EditProductHandler : IRequestHandler<EditProductForm, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EditProductHandler> _logger;

        public EditProductHandler(ApplicationDbContext context, IMapper mapper, ILogger<EditProductHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditProductForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditProductValidator(_context);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                Product product = await _context.Products
                    .Include(x => x.Attributes)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                product.Sku = request.Sku;
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.CrossedOutPrice = request.CrossedOutPrice;
                product.Length = request.Length;
                product.Width = request.Width;
                product.Height = request.Height;
                product.DimensionUnits = request.DimensionUnits;
                product.Weight = request.Weight;
                product.WeightUnits = request.WeightUnits;
                product.MinPurchaseQuantity = request.MinPurchaseQuantity;
                product.MaxPurchaseQuantity = request.MaxPurchaseQuantity;
                product.InStock = request.InStock;
                product.Unlimited = request.Unlimited;
                product.Enabled = request.Enabled;

                List<ProductCategories> oldCategories = await _context.ProductCategories
                    .Where(x => x.ProductId == request.ProductId)
                    .ToListAsync(cancellationToken);
                List<ProductCategories> newCategories = request.CategoryIds
                    .Select(categoryId => new ProductCategories
                    {
                        CategoryId = categoryId,
                        ProductId = request.ProductId,
                    })
                    .ToList();

                _context.ProductCategories.RemoveRange(oldCategories);
                _context.ProductCategories.AddRange(newCategories);

                _context.ProductAttributes.RemoveRange(product.Attributes);
                product.Attributes = _mapper.Map<List<ProductAttribute>>(request.Attributes);

                _context.Products.Update(product);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating product product {id}", request.ProductId);
                throw;
            }
        }
    }
}
