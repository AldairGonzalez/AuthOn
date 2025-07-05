using AuthOn.Application.Configurations;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.Emails;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace AuthOn.Application.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly GmailConfiguration _settings;
        private readonly SmtpClient _smtpClient;

        public EmailSenderService(IOptions<GmailConfiguration> settings)
        { 
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = _settings.Port,
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(_settings.UserEmail, _settings.Password)
            };
        }

        public async Task<ErrorOr<Email>> SendEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(_settings.UserEmail!),
                    To = { email.DestinationEmail!.Value },
                    Subject = email.Subject,
                    Body = email.Message,
                    IsBodyHtml = true
                };
                await _smtpClient.SendMailAsync(message, cancellationToken);
                email.UpdateStateSent();
                return email;
            }
            catch (SmtpException ex)
            {
                return EmailSenderServiceErrors.EmailSenderService.EmailSendingFailed(ex.Message);
            }
        }
    }
}