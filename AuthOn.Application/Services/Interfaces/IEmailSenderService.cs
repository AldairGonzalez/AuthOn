using AuthOn.Domain.Entities.Emails;
using ErrorOr;

namespace AuthOn.Application.Services.Interfaces
{

    public interface IEmailSenderService
    {
        Task<ErrorOr<Email>> SendEmailAsync(Email email, CancellationToken cancellationToken = default);
    }
}