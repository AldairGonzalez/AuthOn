using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Infrastructure.Persistence.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }
    }
}