using ErrorOr;
using MediatR;

namespace AuthOn.Application.Entities.Users.Commands.Create
{
    public record CreateUserCommand(
        string UserName,
        string Email,
        string Password) : IRequest<ErrorOr<Unit>>;
}