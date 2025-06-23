using AuthOn.Domain.Entities.Users;

namespace AuthOn.Domain.Entities.UserTokens
{
    public interface IUserTokenRepository
    {
        Task AddAsync(UserToken userToken, CancellationToken cancellationToken);
        Task DeleteAsync(UserToken userToken);
        Task<List<UserToken>> FindAllAsync(CancellationToken cancellationToken, byte? tokenType = null, UserId? userId = null, bool? isUsed = null, bool? isExpired = null);
        Task<UserToken?> GetAsync(UserId userId, string token, CancellationToken cancellationToken);
        Task Update(UserToken userToken);
    }
}