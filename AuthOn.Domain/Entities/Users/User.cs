﻿using AuthOn.Domain.Primitives;
using AuthOn.Domain.ValueObjects;

namespace AuthOn.Domain.Entities.Users
{
    public sealed class User : AggregateRoot
    {
        #region Properties

        public UserId Id { get; private set; }
        public UserName UserName { get; private set; }
        public EmailAddress Email { get; private set; }
        public string HashedPassword { get; private set; } = string.Empty;
        public bool EmailConfirmed { get; private set; }
        public bool IsBlocked { get; private set; }
        public byte AuthenticationAttempts { get; private set; }
        public DateTime? DeletionDate { get; private set; }
        public DateTime RecordCreationMoment { get; private set; }
        public DateTime RecordUpdateMoment { get; private set; }

        #endregion

        #region Builders

        private User(UserId id, UserName userName, EmailAddress email, string hashedPassword)
        {
            Id = id;
            UserName = userName;
            Email = email;
            HashedPassword = hashedPassword;
            EmailConfirmed = false;
            IsBlocked = true;
            AuthenticationAttempts = 0;
            RecordCreationMoment = DateTime.UtcNow;
            RecordUpdateMoment = DateTime.UtcNow;
        }

        private User()
        {

        }

        #endregion

        #region Methods

        public static User Create(UserName userName, EmailAddress email, string hashedPassword)
        {
            return new User(new UserId(Guid.NewGuid()), userName, email, hashedPassword);
        }

        #endregion
    }
}