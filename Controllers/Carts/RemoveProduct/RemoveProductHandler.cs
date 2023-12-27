using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Carts.RemoveProduct
{
    public record RemoveProductRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID who owns the cart.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Product ID to delete from the cart.
        /// </summary>
        public int ProductId { get; set; }
    }

    public class RemoveProductHandler : IRequestHandler<RemoveProductRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RemoveProductHandler> _logger;

        public RemoveProductHandler(ApplicationDbContext context, ILogger<RemoveProductHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(RemoveProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Cart item = await _context.Carts
                    .FirstOrDefaultAsync(x => x.UserId ==  request.UserId
                        && x.ProductId == request.ProductId, cancellationToken)
                    ?? throw new BadRequestException($"Product {request.ProductId} is not in the cart");

                _context.Carts.Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting product {productId} from cart", request.ProductId);
                throw;
            }
        }
    }
}
