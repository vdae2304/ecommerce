using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.Email
{
    public class ConfirmEmailRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Email confirmation token.
        /// </summary>
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
