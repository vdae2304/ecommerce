using AutoMapper;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.Infrastructure.Implementation
{
    /// <inheritdoc/>
    public class SecurityManager : ISecurityManager
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public SecurityManager(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public Authentication GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Authentication:Jwt:Key"] ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now
                .AddMinutes(_config.GetValue<int>("Authentication:Jwt:Lifetime"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = new JwtSecurityToken(
                issuer: _config["Authentication:Jwt:Issuer"],
                audience: _config["Authentication:Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new Authentication
            {
                TokenType = JwtBearerDefaults.AuthenticationScheme,
                Token = tokenHandler.WriteToken(securityToken),
                Expires = expires,
                User = _mapper.Map<UserProfile>(user)
            };
        }

        /// <inheritdoc/>
        public string Encrypt(string value)
        {
            var aes = Aes.Create();
            var securityKey = Encoding.UTF8.GetBytes(_config["SecurityManager:SecurityKey"] ?? string.Empty);
            var IV = aes.IV;

            var stream = new MemoryStream();
            stream.Write(IV, 0, IV.Length);

            var encryptor = aes.CreateEncryptor(securityKey, IV);
            var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
            var streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(value);
            streamWriter.Close();

            return Convert.ToBase64String(stream.ToArray());
        }

        /// <inheritdoc/>
        public string Decrypt(string encryptedValue)
        {
            var aes = Aes.Create();
            var securityKey = Encoding.UTF8.GetBytes(_config["SecurityManager:SecurityKey"] ?? string.Empty);
            var IV = new byte[aes.IV.Length];

            var stream = new MemoryStream(Convert.FromBase64String(encryptedValue));
            stream.Read(IV, 0, IV.Length);

            var decryptor = aes.CreateDecryptor(securityKey, IV);
            var cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
            var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}
