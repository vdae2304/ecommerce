using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Products.EditProduct
{
    public record EditProductForm : IRequest<ActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        [JsonIgnore]
        public int ProductId { get; set; }

        /// <summary>
        /// (Optional) An unique identifier for the product.
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// (Optional) Product name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// (Optional) Product description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// (Optional) Product price.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// (Optional) If not null, display a price to compare to.
        /// </summary>
        public decimal? CrossedOutPrice { get; set; }

        /// <summary>
        /// (Optional) Product width.
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// (Optional) Product height.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// (Optional) Product length.
        /// </summary>
        public double? Length { get; set; }

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
        /// (Optional) Minimum allowed purchase quantity.
        /// </summary>
        public int? MinPurchaseQuantity { get; set; }

        /// <summary>
        /// (Optional) Maximum allowed purchase quantity.
        /// </summary>
        public int? MaxPurchaseQuantity { get; set; }

        /// <summary>
        /// (Optional) Number of products in stock. Set to null if does not apply
        /// or unlimited.
        /// </summary>
        public int? InStock { get; set; }

        /// <summary>
        /// (Optional) Whether the product is enabled or not.
        /// </summary>
        public bool? Enabled { get; set; }
    }

    public class EditProductHandler : IRequestHandler<EditProductForm, ActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditProductHandler> _logger;

        public EditProductHandler(ApplicationDbContext context, ILogger<EditProductHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(EditProductForm request, CancellationToken cancellationToken)
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
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                if (request.Sku != null)
                {
                    product.Sku = request.Sku;
                }
                if (request.Name != null)
                {
                    product.Name = request.Name;
                }
                if (request.Description != null)
                {
                    product.Description = request.Description;
                }
                if (request.Price != null)
                {
                    product.Price = request.Price.Value;
                }
                if (request.CrossedOutPrice != null)
                {
                    product.CrossedOutPrice = request.CrossedOutPrice;
                }
                if (request.Width != null)
                {
                    product.Width = request.Width;
                }
                if (request.Height != null)
                {
                    product.Height = request.Height;
                }
                if (request.Length != null)
                {
                    product.Length = request.Length;
                }
                if (request.DimensionUnits != null)
                {
                    product.DimensionUnits = request.DimensionUnits;
                }
                if (request.Weight != null)
                {
                    product.Weight = request.Weight;
                }
                if (request.WeightUnits != null)
                {
                    product.WeightUnits = request.WeightUnits;
                }
                if (request.MinPurchaseQuantity != null)
                {
                    product.MinPurchaseQuantity = request.MinPurchaseQuantity.Value;
                }
                if (request.MaxPurchaseQuantity != null)
                {
                    product.MaxPurchaseQuantity = request.MaxPurchaseQuantity.Value;
                }
                if (request.InStock != null)
                {
                    product.InStock = request.InStock;
                }
                if (request.Enabled != null)
                {
                    product.Enabled = request.Enabled.Value;
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = $"Ok."
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
