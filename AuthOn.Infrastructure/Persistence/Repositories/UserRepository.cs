using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByEmailAsync(EmailAddress email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<User?> GetByIdAsync(UserId id)
        {
            return await _context.Users.FindAsync(id);
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}