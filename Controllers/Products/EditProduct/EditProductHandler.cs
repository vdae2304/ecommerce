using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.EditProduct
{
    public class EditProductHandler : IRequestHandler<EditProductRequest, IActionResult>
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

        public async Task<IActionResult> Handle(EditProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditProductValidator(_context);
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                Product product = await _context.Products
                    .Include(x => x.Attributes)
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException();

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
