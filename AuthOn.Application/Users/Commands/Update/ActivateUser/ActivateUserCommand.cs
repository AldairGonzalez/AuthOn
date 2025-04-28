using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.ActivateUser
{
    public record ActivateUserCommand(
    Guid UserId,
    long EmailId) : IRequest<ErrorOr<Unit>>;
}