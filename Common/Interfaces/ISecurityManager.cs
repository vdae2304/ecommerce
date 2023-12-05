using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;

namespace Ecommerce.Common.Interfaces
{
    public interface ISecurityManager
    {
        public Authentication GenerateAccessToken(ApplicationUser user);
    }
}
