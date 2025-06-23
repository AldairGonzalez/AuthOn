using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Infrastructure.Persistence.Repositories
{
    public class UserTokenRepository(ApplicationDbContext context) : IUserTokenRepository
    {
        private readonly ApplicationDbContext _context = context;

        #region Methods

        public async Task AddAsync(UserToken userToken, CancellationToken cancellationToken)
        {
            await _context.UserTokens.AddAsync(userToken, cancellationToken);
        }

        public async Task DeleteAsync(UserToken userToken)
        {
           _context.UserTokens.Remove(userToken);
            await Task.CompletedTask;
        }

        public async Task<List<UserToken>> FindAllAsync(CancellationToken cancellationToken, byte? tokenType = null, UserId? userId = null, bool? isUsed = null, bool? isExpired = null)
        {
            var query = _context.UserTokens.AsQueryable();

            if (tokenType.HasValue)
                query = query.Where(ut => ut.TokenTypeId == tokenType.Value);

            if (userId is not null)
                query = query.Where(ut => ut.UserId == userId);

            if (isUsed.HasValue)
                query = query.Where(ut => ut.IsUsed == isUsed.Value);

            if (isExpired.HasValue)
                query = query.Where(ut => ut.IsExpired == isExpired.Value);

            return await query.ToListAsync(cancellationToken);
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

        #endregion
    }
}