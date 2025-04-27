using ErrorOr;

namespace AuthOn.Shared.Errors.ApplicationErrors
{
    public static partial class EmailErrors
    {
        public static class Email
        {
            public static Error EmailNotFound(long emailId) =>
                Error.Validation("Email.Id", $"Email with Id = {emailId} was not found.");

            public static Error EmailWithBadFormat =>
                Error.Validation("Email.DestinationEmail", "Destination Email has not valid format.");

            public static Error SubjectIsNullOrWhiteSpace =>
                Error.Validation("Email.Subject", "Subject cannot be empty.");

            public static Error MessageIsNullOrWhiteSpace =>
                Error.Validation("Email.Message", "Message cannot be empty.");
        }
    }
}