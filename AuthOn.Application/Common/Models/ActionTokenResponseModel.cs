using ErrorOr;

namespace AuthOn.Application.Common.Models
{
    public class ActionTokenResponseModel(ErrorOr<Guid?> userId, ErrorOr<long?> emailId)
    {
        public ErrorOr<Guid?> UserId { get; } = userId;
        public ErrorOr<long?> EmailId { get; } = emailId;

        public bool HasErrors => UserId.IsError || EmailId.IsError;

        public IEnumerable<Error> GetErrors()
        {
            var errors = new List<Error>();
            if (UserId.IsError) errors.AddRange(UserId.Errors);
            if (EmailId.IsError) errors.AddRange(EmailId.Errors);
            return errors;
        }
    }
}