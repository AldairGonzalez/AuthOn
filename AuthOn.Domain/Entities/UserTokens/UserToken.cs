using AuthOn.Domain.Entities.TokenTypes;
using AuthOn.Domain.Entities.Users;

namespace AuthOn.Domain.Entities.UserTokens
{
    public sealed class UserToken
    {
        #region Properties

        public long Id { get; private set; }
        public byte TokenTypeId { get; private set; }
        public UserId? UserId { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public bool IsUsed { get; private set; }
        public bool IsExpired { get; private set; }
        public DateTime RecordCreationMoment { get; private set; }
        public DateTime RecordUpdateMoment { get; private set; }

        #endregion

        #region Navigation Properties

        public TokenType? TokenType { get; private set; }
        public User? User { get; private set; }

        #endregion

        #region Constructors

        public UserToken() { }

        public UserToken(byte tokenTypeId, UserId userId, string token)
        {
            TokenTypeId = tokenTypeId;
            UserId = userId;
            Token = token;
            IsUsed = false;
            IsExpired = false;
            RecordCreationMoment = DateTime.UtcNow;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        #endregion

        #region Methods

        public static UserToken Create(byte tokenTypeId, Guid userId, string token)
        {
            return new UserToken(tokenTypeId, new UserId(userId), token);
        }

        public void MarkAsUsed()
        {
            IsUsed = true;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        #endregion
    }
}