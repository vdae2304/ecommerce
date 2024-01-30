using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Orders.GetOrder
{
    public class GetOrderHandler : IRequestHandler<GetOrderRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<GetOrderHandler> _logger;

        public GetOrderHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<GetOrderHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        private void DecryptAddress(Address address)
        {
            address.Recipient = _securityManager.Decrypt(address.Recipient);
            address.Phone = _securityManager.Decrypt(address.Phone);
        }

        private void DecryptPaymentMethod(PaymentMethod paymentMethod)
        {
            paymentMethod.CardOwner = _securityManager.Decrypt(paymentMethod.CardOwner);
            paymentMethod.CardNumber = _securityManager.Decrypt(paymentMethod.CardNumber);
            paymentMethod.CVV = _securityManager.Decrypt(paymentMethod.CVV);
            paymentMethod.ExpiryMonth = _securityManager.Decrypt(paymentMethod.ExpiryMonth);
            paymentMethod.ExpiryYear = _securityManager.Decrypt(paymentMethod.ExpiryYear);
            if (paymentMethod.BillingAddress != null)
            {
                DecryptAddress(paymentMethod.BillingAddress);
            }
        }

        public async Task<IActionResult> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Order order = await _context.Orders
                    .Include(x => x.Items)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Thumbnail)
                    .Include(x => x.Items)
                      .ThenInclude(x => x.Product)
                          .ThenInclude(x => x.GalleryImages)
                    .Include(x => x.Items)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Categories)
                    .Include(x => x.Items)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Attributes)
                    .Include(x => x.PaymentMethod)
                        .ThenInclude(x => x!.BillingAddress)
                    .Include(x => x.ShippingAddress)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException();

                if (order.PaymentMethod != null)
                {
                    DecryptPaymentMethod(order.PaymentMethod);
                }
                if (order.ShippingAddress != null)
                {
                    DecryptAddress(order.ShippingAddress);
                }

                return new OkObjectResult(new Response<Order>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting details for order {orderId}", request.OrderId);
                throw;
            }
        }
    }
}
