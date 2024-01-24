using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.Password
{
    public record ChangePasswordRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID.
        /// </summary>
        [BindNever]
        [FromRoute]
        public int UserId { get; set; }
        /// <summary>
        /// Current password.
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        /// <summary>
        /// New password.
        /// </summary>
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
