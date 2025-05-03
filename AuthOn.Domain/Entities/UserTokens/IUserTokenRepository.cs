using AuthOn.Domain.Entities.Users;

namespace AuthOn.Domain.Entities.UserTokens
{
    public interface IUserTokenRepository
    {
        Task<UserToken?> GetAsync(UserId userId, string token, CancellationToken cancellationToken);
        Task AddAsync(UserToken userToken, CancellationToken cancellationToken);
        Task Update(UserToken userToken);
    }
}