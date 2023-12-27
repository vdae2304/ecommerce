using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Payment.SearchPaymentMethods
{
    public record PaymentMethodFilters : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the payment methods.
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

    public class SearchPaymentMethodsHandler : IRequestHandler<PaymentMethodFilters, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<SearchPaymentMethodsHandler> _logger;

        public SearchPaymentMethodsHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<SearchPaymentMethodsHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(PaymentMethodFilters filters, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.PaymentMethods
                    .Include(x => x.BillingAddress)
                    .Where(x => x.UserId == filters.UserId)
                    .AsSplitQuery()
                    .AsNoTracking();

                int total = await query.CountAsync(cancellationToken);
                List<PaymentMethod> paymentMethods = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(filters.Offset)
                    .Take(filters.Limit)
                    .ToListAsync(cancellationToken);

                foreach (PaymentMethod paymentMethod in paymentMethods)
                {
                    paymentMethod.CardOwner = _securityManager.Decrypt(paymentMethod.CardOwner);
                    paymentMethod.CardNumber = _securityManager.Decrypt(paymentMethod.CardNumber);
                    paymentMethod.CVV = _securityManager.Decrypt(paymentMethod.CVV);
                    paymentMethod.ExpiryMonth = _securityManager.Decrypt(paymentMethod.ExpiryMonth);
                    paymentMethod.ExpiryYear = _securityManager.Decrypt(paymentMethod.ExpiryYear);
                    if (paymentMethod.BillingAddress != null)
                    {
                        paymentMethod.BillingAddress.Recipient =
                            _securityManager.Decrypt(paymentMethod.BillingAddress.Recipient);
                        paymentMethod.BillingAddress.Phone =
                            _securityManager.Decrypt(paymentMethod.BillingAddress.Phone);
                    }
                }

                return new OkObjectResult(new Response<SearchItems<PaymentMethod>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new SearchItems<PaymentMethod>
                    {
                        Total = total,
                        Offset = filters.Offset,
                        Limit = filters.Limit,
                        Items = paymentMethods
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting payment methods for user {userId}", filters.UserId);
                throw;
            }
        }
    }
}
