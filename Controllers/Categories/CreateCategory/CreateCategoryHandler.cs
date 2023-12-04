using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Categories.CreateCategory
{
    public record CreateCategoryForm : IRequest<IActionResult>
    {
        /// <summary>
        /// ID of the parent category, if any.
        /// Default is to set as root category.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description.
        /// </summary>
        [JsonRequired]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ID of the products to assign to the category.
        /// </summary>
        public IEnumerable<int> ProductIds { get; set; } = new List<int>();

        /// <summary>
        /// Whether the category is enabled or not.
        /// Default is true.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
     
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryForm, IActionResult>
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

        public async Task<IActionResult> Handle(CreateCategoryForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateCategoryValidator(_context);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

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
