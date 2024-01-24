using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.SignUp
{
    public record SignUpRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Username.
        /// </summary>
        [Required]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
