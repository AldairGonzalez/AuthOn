using AuthOn.Application.Services;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Primitives;
using AuthOn.Shared.Constants;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.ActivateUser
{
    internal sealed class ActivateUserCommandHandler(
        IUserRepository userRepository,
        IEmailTemplateService emailTemplateService,
        IEmailSenderService emailSenderService,
        IEmailRepository emailRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ActivateUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService ?? throw new ArgumentNullException(nameof(emailTemplateService));
        private readonly IEmailSenderService _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
        private readonly IEmailRepository _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));

        public async Task<ErrorOr<Unit>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Validations

                var user = await _userRepository.GetByIdAsync(new UserId(request.UserId));
                if (user == null)
                {
                    return UserErrors.User.UserNotFound(request.UserId);
                }

                if (!user.IsLocked)
                {
                    return UserErrors.User.UserAlreadyActivated;
                }

                var email = await _emailRepository.GetByIdAsync(request.EmailId);

                if (email == null)
                {
                    return EmailErrors.Email.EmailNotFound(request.EmailId);
                }

                #endregion

                #region Process

                user.ActivateUser();

                await _userRepository.UpdateAsync(user);

                email.MarkAsVisualized();

                await _emailRepository.UpdateAsync(email);

                var newEmail = Email.Create(
                    destinationEmail: user.Email,
                    subject: EmailConstants.TitleUserActivated,
                    message: _emailTemplateService.GenerateInformativeEmailActivatedUser(user.UserName.Value));

                await _emailRepository.AddAsync(newEmail);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var emailResult = await _emailSenderService.SendEmailAsync(newEmail, cancellationToken);

                if (emailResult.IsError)
                {
                    newEmail.UpdateStateFailed();

                    await _emailRepository.UpdateAsync(newEmail);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return emailResult.Errors;
                }

                await _emailRepository.UpdateAsync(emailResult.Value);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;

                #endregion
            }
            catch (Exception ex)
            {
                return CommandHandlerErrors.User.UnexpectedErrorWhenActivatingUser(ex.Message);
            }
        }
    }
}