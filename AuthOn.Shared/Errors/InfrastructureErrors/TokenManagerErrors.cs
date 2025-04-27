using ErrorOr;
using System.Security.Claims;

namespace AuthOn.Shared.Errors.InfrastructureErrors
{
    public static class TokenManagerErrors
    {
        public static Error ConfigurationNotFound(string tokenType) =>
            Error.Validation("TokenManager.Configuration", $"No configuration found for token type: {tokenType}");
        
        public static Error ClaimNotFound(string claimTypes) =>
            Error.Validation($"TokenManager.{claimTypes}", $"The claim '{claimTypes}' was not found in the token.");

        public static Error InvalidToken =>
            Error.Validation("TokenManager.token", "The token is invalid.");

        public static Error InvalidTokenFormat =>
            Error.Validation("TokenManager.jwtToken", "The token has not valid format.");

        public static Error TokenExpired =>
            Error.Validation("TokenManager.token", "The token has expired.");

        public static Error UnknownError =>
            Error.Validation("Unknown Error", "An unknown error occurred while validating the token.");
    }
}