using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Products.CreateProduct
{
    public record CreateProductForm : IRequest<ActionResult>
    {
        /// <summary>
        /// An unique identifier for the product.
        /// </summary>
        [Required]
        public string Sku { get; set; } = string.Empty;

        /// <summary>
        /// Product name.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product description.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Product price.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// ID of the categories the product is assigned to.
        /// </summary>
        public IEnumerable<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// Product attributes.
        /// </summary>
        public List<CreateAttributeForm> Attributes { get; set; } = new List<CreateAttributeForm>();

        /// <summary>
        /// Product width.
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// Product height.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Product length.
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// Dimension units.
        /// </summary>
        public DimensionUnits? DimensionUnits { get; set; }

        /// <summary>
        /// Product weight.
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// Weight units.
        /// </summary>
        public WeightUnits? WeightUnits { get; set; }

        /// <summary>
        /// Minimum allowed purchase quantity.
        /// </summary>
        public int MinPurchaseQuantity { get; set; } = 1;

        /// <summary>
        /// Maximum allowed purchase quantity.
        /// </summary>
        public int MaxPurchaseQuantity { get; set; } = 1;

        /// <summary>
        /// Number of products in stock. Set to null if does not apply or
        /// unlimited.
        /// </summary>
        public int? InStock { get; set; }

        /// <summary>
        /// Whether the product is enabled or not.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
    
    public record CreateAttributeForm : IRequest<ActionResult>
    {
        /// <summary>
        /// Attribute name.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Attribute value.
        /// </summary>
        public string? Value { get; set; }
    }

    public class CreateProductHandler : IRequestHandler<CreateProductForm, ActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateProductHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(CreateProductForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateProductValidator(_context);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                Product product = _mapper.Map<Product>(request);
                product.Categories = await _context.Categories
                    .Where(x => request.CategoryIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
                product.Attributes = _mapper.Map<List<ProductAttribute>>(request.Attributes);

                _context.Products.Add(product);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = $"Ok.",
                    Data = new CreatedId { Id = product.Id }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating product {sku}", request.Sku);
                throw;
            }
        }
    }
}
