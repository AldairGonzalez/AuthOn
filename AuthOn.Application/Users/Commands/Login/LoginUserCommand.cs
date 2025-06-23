using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Login
{
    public record LoginUserCommand(
        string Email,
        string Password) : IRequest<ErrorOr<string>>;
}