using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Payment.CreatePaymentMethod
{
    public class CreatePaymentMethodHandler : IRequestHandler<CreatePaymentMethodRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<CreatePaymentMethodHandler> _logger;

        public CreatePaymentMethodHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<CreatePaymentMethodHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreatePaymentMethodRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreatePaymentMethodValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                var billingAddress = await _context.Addresses
                    .FirstOrDefaultAsync(x => x.Id == request.BillingAddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new BadRequestException($"Address {request.BillingAddressId} does not exist");
                
                var paymentMethod = new PaymentMethod
                {
                    UserId = request.UserId,
                    CardOwner = _securityManager.Encrypt(request.CardOwner),
                    CardNumber = _securityManager.Encrypt(request.CardNumber),
                    CVV = _securityManager.Encrypt(request.CVV),
                    ExpiryMonth = _securityManager.Encrypt(request.ExpiryMonth.ToString("00")),
                    ExpiryYear = _securityManager.Encrypt(request.ExpiryYear.ToString("0000")),
                    BillingAddress = billingAddress
                };
                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new CreatedId { Id = paymentMethod.Id }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating payment method");
                throw;
            }
        }
    }
}
