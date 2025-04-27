using ErrorOr;

namespace AuthOn.Shared.Errors.ApplicationErrors
{
    public static partial class UserErrors
    {
        public static class User
        {
            public static Error EmailAlreadyExists =>
                Error.Validation("User.Email", "Email already exists.");

            public static Error EmailWithBadFormat =>
                Error.Validation("User.Email", "Email has not valid format.");

            public static Error PasswordWithBadFormat =>
                Error.Validation("User.Password", "Password has not valid format.");

            public static Error UserNameWithBadFormat =>
                Error.Validation("User.UserName", "User name has not valid format.");
        }
    }
}