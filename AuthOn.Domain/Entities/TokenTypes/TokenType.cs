using AuthOn.Domain.Entities.UserTokens;

namespace AuthOn.Domain.Entities.TokenTypes
{
    public sealed class TokenType
    {
        #region Properties

        public byte Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int ExpirationTimeInHours { get; private set; }

        #endregion

        #region Navigation Properties

        public List<UserToken> UserTokens { get; private set; } = [];

        #endregion

        #region Static Properties

        public static TokenType ActivationToken => new(1, "ACTIVATION_TOKEN", 48);

        #endregion

        #region Constructors

        public TokenType() { }

        public TokenType(byte id, string name, int expirationTimeInHours)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ExpirationTimeInHours = expirationTimeInHours;
        }

        #endregion
    }
}