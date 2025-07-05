using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.ActivateUser
{
    public record ActivateUserCommand(
    Guid UserId,
    long EmailId,
    string Token) : IRequest<ErrorOr<Unit>>;
}