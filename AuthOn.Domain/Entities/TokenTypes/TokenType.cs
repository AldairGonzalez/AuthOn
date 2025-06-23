using AuthOn.Domain.Entities.UserTokens;

namespace AuthOn.Domain.Entities.TokenTypes
{
    public sealed class TokenType
    {
        #region Properties

        public byte Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public double ExpirationTimeInHours { get; private set; }

        #endregion

        #region Navigation Properties

        public List<UserToken> UserTokens { get; private set; } = [];

        #endregion

        #region Static Properties

        public static TokenType AccessToken => new(1, "ACCESS_TOKEN", 0.25);

        public static TokenType ActivationToken => new(2, "ACTIVATION_TOKEN", 48);

        public static TokenType RefreshToken => new(3, "REFRESH_TOKEN", 720);

        #endregion

        #region Constructors

        public TokenType() { }

        public TokenType(byte id, string name, double expirationTimeInHours)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ExpirationTimeInHours = expirationTimeInHours;
        }

        #endregion
    }
}