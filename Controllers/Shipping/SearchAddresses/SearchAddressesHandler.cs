using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Shipping.SearchAddresses
{
    public record AddressFilters : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the addresses.
        /// </summary>
        [BindNever]
        public int UserId { get; set; }

        /// <summary>
        /// Number of items to skip at the beginning.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Maximum number of items to return.
        /// </summary>
        public int Limit { get; set; } = 100;
    }

    public class SearchAddressesHandler : IRequestHandler<AddressFilters, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<SearchAddressesHandler> _logger;

        public SearchAddressesHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<SearchAddressesHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddressFilters filters, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.ShippingAddresses.Where(x => x.UserId == filters.UserId);

                int total = await query.CountAsync(cancellationToken);
                List<ShippingAddress> shippingAddresses = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(filters.Offset)
                    .Take(filters.Limit)
                    .ToListAsync(cancellationToken);

                foreach (ShippingAddress shippingAddress in shippingAddresses)
                {
                    shippingAddress.Name = _securityManager.Decrypt(shippingAddress.Name);
                    shippingAddress.Phone = _securityManager.Decrypt(shippingAddress.Phone);
                }

                return new OkObjectResult(new Response<SearchItems<ShippingAddress>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new SearchItems<ShippingAddress>
                    {
                        Total = total,
                        Offset = filters.Offset,
                        Limit = filters.Limit,
                        Items = shippingAddresses
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting shipping addresses for user {userId}", filters.UserId);
                throw;
            }
        }
    }
}
