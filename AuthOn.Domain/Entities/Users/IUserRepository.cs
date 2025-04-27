using AuthOn.Domain.ValueObjects;

namespace AuthOn.Domain.Entities.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(UserId id);
        Task<User?> GetByEmailAsync(EmailAddress email);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}