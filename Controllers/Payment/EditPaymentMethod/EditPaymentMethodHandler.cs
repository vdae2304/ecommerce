using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Payment.EditPaymentMethod
{
    public class EditPaymentMethodHandler : IRequestHandler<EditPaymentMethodRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private ISecurityManager _securityManager;
        private readonly ILogger<EditPaymentMethodHandler> _logger;

        public EditPaymentMethodHandler(ApplicationDbContext context, ISecurityManager securityManager, ILogger<EditPaymentMethodHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditPaymentMethodRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditPaymentMethodValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                PaymentMethod paymentMethod = await _context.PaymentMethods
                    .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException();

                if (request.CardOwner != null)
                {
                    paymentMethod.CardOwner = _securityManager.Encrypt(request.CardOwner);
                }
                if (request.CVV != null)
                {
                    paymentMethod.CVV = _securityManager.Encrypt(request.CVV);
                }
                if (request.ExpiryMonth != null)
                {
                    paymentMethod.ExpiryMonth = _securityManager.Encrypt(request.ExpiryMonth.Value.ToString("00"));
                }
                if (request.ExpiryYear != null)
                {
                    paymentMethod.ExpiryYear = _securityManager.Encrypt(request.ExpiryYear.Value.ToString("0000"));
                }
                if (request.BillingAddressId != null)
                {
                    paymentMethod.BillingAddress = await _context.Addresses
                        .FirstOrDefaultAsync(x => x.Id == request.BillingAddressId &&
                            x.UserId == request.UserId, cancellationToken)
                        ?? throw new BadRequestException($"Address {request.BillingAddressId} does not exist");
                }

                _context.PaymentMethods.Update(paymentMethod);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in editing payment method {paymentMethodId}", request.PaymentMethodId);
                throw;
            }
        }
    }
}
