using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;

namespace Ecommerce.Common.Interfaces
{
    /// <summary>
    /// Security manager.
    /// </summary>
    public interface ISecurityManager
    {
        /// <summary>
        /// Generate an access token for the given user.
        /// </summary>
        /// <param name="user">User to create a token for.</param>
        /// <returns>Access token.</returns>
        public Authentication GenerateAccessToken(ApplicationUser user);

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="value">Value to encrypt.</param>
        /// <returns>Encrypted valued.</returns>
        public string Encrypt(string value);

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedValue">Value to decrypt.</param>
        /// <returns>Decrypted value.</returns>
        public string Decrypt(string encryptedValue);
    }
}
