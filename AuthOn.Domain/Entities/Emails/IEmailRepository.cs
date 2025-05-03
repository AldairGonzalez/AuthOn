namespace AuthOn.Domain.Entities.Emails
{
    public interface IEmailRepository
    {
        Task AddAsync(Email email, CancellationToken cancellationToken);
        Task<Email?> GetByIdAsync(long emailId, CancellationToken cancellationToken);
        Task Update(Email email);
    }
}