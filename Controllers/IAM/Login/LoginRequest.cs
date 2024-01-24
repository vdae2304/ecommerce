using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.Login
{
    public record LoginRequest : IRequest<IActionResult>
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
    }
}
