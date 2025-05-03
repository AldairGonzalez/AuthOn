using AuthOn.Domain.ValueObjects;

namespace AuthOn.Domain.Entities.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken);
        Task Update(User user);
    }
}