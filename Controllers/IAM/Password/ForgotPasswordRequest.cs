using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.Password
{
    public record ForgotPasswordRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
