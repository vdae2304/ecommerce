using Ecommerce.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Common.Models.IAM
{
    /// <inheritdoc/>
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        /// <summary>
        /// Datetime of creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Datetime of last modification.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
