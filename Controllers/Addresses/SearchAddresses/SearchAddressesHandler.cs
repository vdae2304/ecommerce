using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Addresses.SearchAddresses
{
    public class SearchAddressesHandler : IRequestHandler<SearchAddressesRequest, IActionResult>
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

        public async Task<IActionResult> Handle(SearchAddressesRequest filters, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Addresses
                    .Where(x => x.UserId == filters.UserId)
                    .AsNoTracking();

                int total = await query.CountAsync(cancellationToken);
                List<Address> addresses = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(filters.Offset)
                    .Take(filters.Limit)
                    .ToListAsync(cancellationToken);

                foreach (Address address in addresses)
                {
                    address.Recipient = _securityManager.Decrypt(address.Recipient);
                    address.Phone = _securityManager.Decrypt(address.Phone);
                }

                return new OkObjectResult(new Response<SearchItems<Address>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new SearchItems<Address>
                    {
                        Total = total,
                        Offset = filters.Offset,
                        Limit = filters.Limit,
                        Items = addresses
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting addresses for user {userId}", filters.UserId);
                throw;
            }
        }
    }
}
