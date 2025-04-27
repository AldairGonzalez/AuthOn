namespace AuthOn.Domain.Entities.EmailStates
{
    public interface IEmailStateRepository
    {
        Task GetByIdAsync(byte id);
        Task GetByCodeAsync(string code);
    }
}