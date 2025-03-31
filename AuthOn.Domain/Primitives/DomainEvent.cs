using MediatR;

namespace AuthOn.Domain.Primitives
{
    public record DomainEvent(Guid Id) : INotification;
}