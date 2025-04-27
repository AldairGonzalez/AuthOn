using AuthOn.Application.Configurations;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Shared.Errors.InfrastructureErrors;
using ErrorOr;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthOn.Infrastructure.Services
{
    public class TokenManager(IOptions<Dictionary<string, TokenConfiguration>> tokenConfigurations) : ITokenManager
    {
        private readonly Dictionary<string, TokenConfiguration> _tokenConfigurations = tokenConfigurations.Value ?? throw new ArgumentNullException(nameof(tokenConfigurations));

        #region Properties

        private const string ActivationTokenCode = "ActivationToken";

        #endregion

        #region Methods

        #region Public

        public ErrorOr<string> GenerateActivationToken(Guid userId) => GenerateUserToken(userId, ActivationTokenCode);

        #endregion

        #region Private

        private ErrorOr<string> GenerateUserToken(Guid userId, string tokenType)
        {
            if (!_tokenConfigurations.TryGetValue(tokenType, out var config))
            {
                return TokenManagerErrors.ConfigurationNotFound(tokenType);
            }

            var tokenGenerator = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config.Key);
            var expiration = DateTime.UtcNow.AddHours(config.ExpiresInHours);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([new Claim("UserId", userId.ToString())]),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenGenerator.CreateToken(tokenDescription);
            return tokenGenerator.WriteToken(token);
        }

        #endregion

        #endregion

    }
}