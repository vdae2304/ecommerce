using AutoMapper;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, IActionResult>
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

        public async Task<IActionResult> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateProductValidator(_context);
                await validator.ValidateAndThrowAsync(request, cancellationToken);

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
                    Message = "Ok.",
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
