using ErrorOr;

namespace AuthOn.Shared.Errors.ApplicationErrors
{
    public static partial class UserTokenErrors
    {
        public static class UserToken
        {
            public static Error TokenNotFound(string token) =>
                Error.Validation("UserToken.Token", $"Token with value = '{token}' was not found.");

            public static Error TokenExpired =>
                Error.Validation("UserToken.token", "The token has expired.");

            public static Error InvalidatedToken =>
                Error.Validation("UserToken.token", "This token has been invalidated.");

            public static Error TokenUsed =>
                Error.Validation("UserToken.token", "The token has been invalidated because it was used.");
        }
    }
}