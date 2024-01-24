using AutoMapper;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(ApplicationDbContext context, IMapper mapper,
            ILogger<CreateCategoryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateCategoryValidator(_context);
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                Category category = _mapper.Map<Category>(request);
                category.Products = await _context.Products
                    .Where(x => request.ProductIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                _context.Categories.Add(category);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new CreatedId { Id = category.Id }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating category {name}", request.Name);
                throw;
            }
        }
    }
}
