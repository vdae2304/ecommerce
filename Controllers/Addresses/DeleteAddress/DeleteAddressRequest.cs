using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Addresses.DeleteAddress
{
    public record DeleteAddressRequest : IRequest<IActionResult>
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
