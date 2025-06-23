using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.Login
{
    public record LoginUserCommand(
        string Email,
        string Password) : IRequest<ErrorOr<LoginUserCommandResponse>>;
}