using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.RefreshToken
{
    public record RefreshTokenUserCommand(
        string RefreshToken) : IRequest<ErrorOr<string>>;
}