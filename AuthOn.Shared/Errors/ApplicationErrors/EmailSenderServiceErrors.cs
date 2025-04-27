using ErrorOr;

namespace AuthOn.Shared.Errors.ApplicationErrors
{
    public static partial class EmailSenderServiceErrors
    {
        public static class EmailSenderService
        {
            public static Error EmailSendingFailed(string ex) =>
                Error.Failure("EmailSenderService.EmailSendingFailed", "Failed to send email." + $"\nDetails: {ex}");
        }
    }
}