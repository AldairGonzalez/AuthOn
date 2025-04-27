using ErrorOr;

namespace AuthOn.Application.Services.Interfaces
{
    public interface ITokenManager
    {
        ErrorOr<string> GenerateActivationToken(Guid userId);
    }
}