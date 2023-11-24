using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Categories.CreateCategory
{
    public record CreateCategoryForm : IRequest<ActionResult>
    {
        /// <summary>
        /// ID of the parent category, if any.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ID of the products assigned to the category.
        /// </summary>
        public IEnumerable<int> ProductIds { get; set; } = new List<int>();
    }
     
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryForm, ActionResult>
    {
        private readonly IGenericRepository<Category> _categories;
        private readonly IGenericRepository<Product> _products;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(IGenericRepository<Category> categories, IGenericRepository<Product> products, IMapper mapper,
            ILogger<CreateCategoryHandler> logger)
        {
            _categories = categories;
            _products = products;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(CreateCategoryForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateCategoryValidator(_categories, _products);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                Category category = _mapper.Map<Category>(request);
                category.Products = await _products.AsQueryable()
                    .Where(x => request.ProductIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
                category = await _categories.AddAsync(category, cancellationToken);

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = $"Category created with id {category.Id}"
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
