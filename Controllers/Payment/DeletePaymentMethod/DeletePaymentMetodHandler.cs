using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Payment.DeletePaymentMethod
{
    public class DeletePaymentMethodHandler : IRequestHandler<DeletePaymentMethodRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeletePaymentMethodHandler> _logger;

        public DeletePaymentMethodHandler(ApplicationDbContext context, ILogger<DeletePaymentMethodHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeletePaymentMethodRequest request, CancellationToken cancellationToken)
        {
            try
            {
                PaymentMethod paymentMethod = await _context.PaymentMethods
                    .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId &&
                    x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException();

                _context.PaymentMethods.Remove(paymentMethod);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting payment method {paymentMethodId}", request.PaymentMethodId);
                throw;
            }
        }
    }
}
