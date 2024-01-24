using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Addresses.GetAddress
{
    public record GetAddressRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Address ID.
        /// </summary>
        public int AddressId { get; set; }
    }
}
