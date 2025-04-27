namespace AuthOn.Domain.Entities.Emails
{
    public interface IEmailRepository
    {
        Task AddAsync(Email email);
        Task<Email?> GetByIdAsync(long emailId);
        Task UpdateAsync(Email email);
    }
}