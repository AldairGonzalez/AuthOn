using AuthOn.Domain.Entities.UserTokens;
using AuthOn.Domain.ValueObjects;

namespace AuthOn.Domain.Entities.Users
{
    public sealed class User
    {
        #region Properties

        public UserId? Id { get; private set; }
        public UserName? UserName { get; private set; }
        public EmailAddress? Email { get; private set; }
        public string HashedPassword { get; private set; } = string.Empty;
        public bool EmailConfirmed { get; private set; }
        public bool IsLocked { get; private set; }
        public byte AuthenticationAttempts { get; private set; }
        public DateTime? DeletionDate { get; private set; }
        public DateTime RecordCreationMoment { get; private set; }
        public DateTime RecordUpdateMoment { get; private set; }

        #endregion

        #region Navigation Properties

        public List<UserToken> UserTokens { get; private set; } = [];

        #endregion

        #region Constructors

        public User() { }

        private User(UserId id, UserName userName, EmailAddress email, string hashedPassword)
        {
            Id = id;
            UserName = userName;
            Email = email;
            HashedPassword = hashedPassword;
            EmailConfirmed = false;
            IsLocked = true;
            AuthenticationAttempts = 0;
            RecordCreationMoment = DateTime.UtcNow;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        #endregion

        #region Methods

        public static User Create(UserName userName, EmailAddress email, string hashedPassword)
        {
            return new User(new UserId(Guid.NewGuid()), userName, email, hashedPassword);
        }

        public void ActivateUser()
        {
            EmailConfirmed = true;
            IsLocked = false;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        public void IncreaseAuthenticationAttempts()
        {
            AuthenticationAttempts++;
            RecordUpdateMoment = DateTime.UtcNow;
            if (AuthenticationAttempts >= 5)
            {
                IsLocked = true;
            }
        }

        public void ResetAuthenticationAttempts()
        {
            AuthenticationAttempts = 0;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        #endregion
    }

}