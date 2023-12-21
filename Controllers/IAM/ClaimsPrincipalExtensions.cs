using System.Security.Claims;

namespace Ecommerce.Controllers.IAM
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            string? claim = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return (claim != null) ? int.Parse(claim) : null;
        }
    }
}
