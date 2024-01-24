using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Payment.GetPaymentMethod
{
    public record GetPaymentMethodRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the payment method.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Payment method ID.
        /// </summary>
        public int PaymentMethodId { get; set; }
    }
}
