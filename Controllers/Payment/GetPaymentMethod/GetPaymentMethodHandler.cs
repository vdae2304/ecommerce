﻿using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Payment.GetPaymentMethod
{
    public class GetPaymentMethodHandler : IRequestHandler<GetPaymentMethodRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<GetPaymentMethodHandler> _logger;

        public GetPaymentMethodHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<GetPaymentMethodHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetPaymentMethodRequest request, CancellationToken cancellationToken)
        {
            try
            {
                PaymentMethod paymentMethod = await _context.PaymentMethods
                    .Include(x => x.BillingAddress)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException();

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

                return new OkObjectResult(new Response<PaymentMethod>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = paymentMethod
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting details for payment method {paymentMethodId}", request.PaymentMethodId);
                throw;
            }
        }
    }
}
