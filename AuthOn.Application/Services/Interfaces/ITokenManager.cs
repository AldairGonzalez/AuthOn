using AuthOn.Application.Common.Models;
using ErrorOr;

namespace AuthOn.Application.Services.Interfaces
{
    public interface ITokenManager
    {
        ErrorOr<double> RefreshTokenExpireInHours { get; }
        ErrorOr<string> GenerateAccessToken(Guid userId);
        ErrorOr<string> GenerateActivationToken(Guid userId, long emailId);
        ErrorOr<string> GenerateRefreshToken(Guid userId);
        ErrorOr<ActionTokenResponseModel> ValidateActivationToken(string token);
        ErrorOr<ActionTokenResponseModel> ValidateRefreshToken(string refreshToken);
    }
}