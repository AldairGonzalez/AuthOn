using ErrorOr;
using MediatR;

namespace AuthOn.Application.Entities.Emails.Commands.Create
{
    public record CreateEmailCommand(
        string DestinationEmail,
        string Subject, 
        string Message) : IRequest<ErrorOr<Unit>>;
}