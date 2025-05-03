namespace AuthOn.Domain.Entities.EmailStates
{
    public interface IEmailStateRepository
    {
        Task GetByIdAsync(byte id, CancellationToken cancellationToken);
        Task GetByCodeAsync(string code, CancellationToken cancellationToken);
    }
}