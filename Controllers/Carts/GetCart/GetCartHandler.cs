using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Carts.GetCart
{
    public record GetCartRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID who owns the cart.
        /// </summary>
        public int UserId { get; set; }
    }

    public class GetCartHandler : IRequestHandler<GetCartRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetCartHandler> _logger;

        public GetCartHandler(ApplicationDbContext context, ILogger<GetCartHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("User {id}", request.UserId);
                List<Cart> items = await _context.Carts
                    .Include(x => x.Product)
                    .ThenInclude(x => x.Thumbnail)
                    .Include(x => x.Product)
                    .ThenInclude(x => x.Categories)
                    .Include(x => x.Product)
                    .ThenInclude(x => x.Attributes)
                    .Where(x => x.UserId == request.UserId)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                _logger.LogInformation("No. items: {count}", items.Count);

                return new OkObjectResult(new Response<List<Cart>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting cart products for user {userId}", request.UserId);
                throw;
            }
        }
    }
}
