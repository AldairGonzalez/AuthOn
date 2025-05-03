using AuthOn.Application.Common.Models;
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

        public ErrorOr<string> GenerateActivationToken(Guid userId, long emailId) => GenerateUserToken(userId, ActivationTokenCode, emailId);

        public ErrorOr<ActionTokenResponseModel> ValidateActivationToken(string token) => ValidateUserToken(token, ActivationTokenCode);

        #endregion

        #region Private

        private ErrorOr<string> GenerateUserToken(Guid userId, string tokenType, long? emailId)
        {
            if (!_tokenConfigurations.TryGetValue(tokenType, out var config))
            {
                return TokenManagerErrors.TokenManager.ConfigurationNotFound(tokenType);
            }

            var tokenGenerator = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config.Key);
            var expiration = DateTime.UtcNow.AddHours(config.ExpiresInHours);

            var claims = new List<Claim>
            {
                new("UserId", userId.ToString())
            };

            if (emailId.HasValue)
            {
                claims.Add(new Claim("EmailId", emailId.Value.ToString()));
            }

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenGenerator.CreateToken(tokenDescription);
            return tokenGenerator.WriteToken(token);
        }

        private ErrorOr<ActionTokenResponseModel> ValidateUserToken(string token, string tokenType)
        {
            try
            {
                if (!_tokenConfigurations.TryGetValue(tokenType, out var config))
                {
                    return TokenManagerErrors.TokenManager.ConfigurationNotFound(tokenType);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(config.Key);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);

                var userIdClaim = claimsPrincipal.FindFirst("UserId");
                ErrorOr<Guid?> userIdResult;
                if (userIdClaim is null)
                {
                    userIdResult = TokenManagerErrors.TokenManager.ClaimNotFound("UserId");
                }
                else if (!Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    userIdResult = TokenManagerErrors.TokenManager.TokenExpired;
                }
                else
                {
                    userIdResult = userId;
                }

                ErrorOr<long?> emailIdResult;
                if (tokenType == ActivationTokenCode)
                {
                    var emailIdClaim = claimsPrincipal.FindFirst("EmailId");
                    if (emailIdClaim is null)
                    {
                        emailIdResult = TokenManagerErrors.TokenManager.ClaimNotFound("EmailId");
                    }
                    else if (!long.TryParse(emailIdClaim.Value, out var emailId))
                    {
                        throw new SecurityTokenExpiredException();
                    }
                    else
                    {
                        emailIdResult = emailId;
                    }
                }
                else
                {
                    emailIdResult = (long?)null;
                }

                var responseModel = new ActionTokenResponseModel(userIdResult, emailIdResult);

                if (responseModel.HasErrors)
                {
                    return responseModel.GetErrors().ToList();
                }

                return responseModel;
            }
            catch (SecurityTokenExpiredException)
            {
                return ErrorOr<ActionTokenResponseModel>.From([TokenManagerErrors.TokenManager.TokenExpired]);
            }
            catch (SecurityTokenException)
            {
                return ErrorOr<ActionTokenResponseModel>.From([TokenManagerErrors.TokenManager.InvalidToken]);
            }
            catch (Exception ex)
            {
                return ErrorOr<ActionTokenResponseModel>.From([TokenManagerErrors.TokenManager.UnknownError(ex.Message)]);
            }
        }

        #endregion

        #endregion

    }
}