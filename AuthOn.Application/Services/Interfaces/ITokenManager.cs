using AuthOn.Application.Common.Models;
using ErrorOr;

namespace AuthOn.Application.Services.Interfaces
{
    public interface ITokenManager
    {
        ErrorOr<string> GenerateAccessToken(Guid userId);
        ErrorOr<string> GenerateActivationToken(Guid userId, long emailId);
        ErrorOr<ActionTokenResponseModel> ValidateActivationToken(string token);
    }
}