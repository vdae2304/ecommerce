using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Products.GetProduct
{
    public record GetProductRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        [Required]
        public int ProductId { get; set; }
    }

    public class GetProductHandler : IRequestHandler<GetProductRequest, ActionResult>
    {
        private readonly IGenericRepository<Product> _products;
        private readonly ILogger<GetProductHandler> _logger;

        public GetProductHandler(IGenericRepository<Product> products, ILogger<GetProductHandler> logger)
        {
            _products = products;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await _products.AsQueryable()
                    .Include(x => x.Thumbnail)
                    .Include(x => x.GalleryImages)
                    .Include(x => x.Categories)
                    .Include(x => x.Attributes)
                    .Include(x => x.DimensionUnits)
                    .Include(x => x.WeightUnits)
                    .Include(x => x.VolumeUnits)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product {request.ProductId} does not exist");

                return new OkObjectResult(new DataResponse<Product> 
                {
                    Success = true,
                    Message = "Ok.",
                    Data = product
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting details for product {productId}", request.ProductId);
                throw;
            }
        }
    }
}
