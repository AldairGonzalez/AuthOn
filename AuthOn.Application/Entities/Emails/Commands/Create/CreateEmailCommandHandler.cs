using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.Primitives;
using AuthOn.Domain.ValueObjects;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Entities.Emails.Commands.Create
{
    internal sealed class CreateEmailCommandHandler : IRequestHandler<CreateEmailCommand, ErrorOr<Unit>>
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmailCommandHandler(IEmailRepository emailRepository, IUnitOfWork unitOfWork)
        {
            _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<Unit>> Handle(CreateEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (EmailAddress.Create(request.DestinationEmail) is not EmailAddress email)
                {
                    return EmailErrors.Email.EmailWithBadFormat;
                }

                if (string.IsNullOrWhiteSpace(request.Subject))
                {
                    return EmailErrors.Email.SubjectIsNullOrWhiteSpace;
                }

                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return EmailErrors.Email.MessageIsNullOrWhiteSpace;
                }

                var newEmail = Email.Create(
                    destinationEmail: email,
                    subject: request.Subject,
                    message: request.Message
                );


                await _emailRepository.AddAsync(newEmail);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return CommandHandlerErrors.Email.UnexpectedErrorWhenCreatingAnEmail(ex.Message);
            }
        }
    }
}