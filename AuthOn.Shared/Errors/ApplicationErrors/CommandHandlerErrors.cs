using ErrorOr;

namespace AuthOn.Shared.Errors.ApplicationErrors
{
    public static partial class CommandHandlerErrors
    {
        public static class User
        {
            public static Error UnexpectedErrorWhenActivatingUser(string ex) =>
                Error.Failure("ActivateUserCommandHandler", $"An unexpected error occurred while activating the user. Details: {ex}");

            public static Error UnexpectedErrorWhenCreatingUser(string ex) =>
                Error.Failure("CreateUserCommandHandler", $"An unexpected error occurred while creating the user. Details: {ex}");
        }

        public static class Email
        {
            public static Error UnexpectedErrorWhenCreatingAnEmail(string ex) =>
                Error.Failure("CreateEmailCommandHandler", $"An unexpected error occurred while creating the email. Details: {ex}");

            public static Error UnexpectedErrorWhenUpdatingAnEmail(string ex) =>
                Error.Failure("UpdateEmailIsVisualizedCommandHandler", $"An unexpected error occurred while updating the email. Details: {ex}");
        }
    }
}
