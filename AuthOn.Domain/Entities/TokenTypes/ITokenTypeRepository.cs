namespace AuthOn.Domain.Entities.TokenTypes
{
    public interface ITokenTypeRepository
    {
        Task GetByIdAsync(byte id, CancellationToken cancellationToken);
        Task GetByCodeAsync(string code, CancellationToken cancellationToken);
    }
}