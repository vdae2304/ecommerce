using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.Password
{
    public record ResetPasswordRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// New password.
        /// </summary>
        [Required]
        public string NewPassword { get; set; } = string.Empty;
        /// <summary>
        /// Password reset token.
        /// </summary>
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
