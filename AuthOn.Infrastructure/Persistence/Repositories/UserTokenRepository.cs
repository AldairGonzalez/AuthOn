using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Infrastructure.Persistence.Repositories
{
    public class UserTokenRepository(ApplicationDbContext context) : IUserTokenRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(UserToken userToken, CancellationToken cancellationToken)
        {
            await _context.UserTokens.AddAsync(userToken, cancellationToken);
        }

        public async Task<UserToken?> GetAsync(UserId userId, string token, CancellationToken cancellationToken)
        {
            return await _context.UserTokens.FirstOrDefaultAsync(ut => ut.UserId == userId && ut.Token == token, cancellationToken);
        }

        public async Task Update(UserToken userToken)
        {
            _context.UserTokens.Update(userToken);
            await Task.CompletedTask;
        }
    }
}