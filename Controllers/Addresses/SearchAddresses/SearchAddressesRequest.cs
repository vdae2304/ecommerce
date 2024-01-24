using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Addresses.SearchAddresses
{
    public record SearchAddressesRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the addresses.
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
}
