using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using AuthOn.Domain.Primitives;
using AuthOn.Shared.Constants;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.ActivateUser
{
    internal sealed class ActivateUserCommandHandler(
        IUserRepository userRepository,
        IUserTokenRepository userTokenRepository,
        IEmailTemplateService emailTemplateService,
        IEmailSenderService emailSenderService,
        IEmailRepository emailRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ActivateUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository ?? throw new ArgumentNullException(nameof(userTokenRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService ?? throw new ArgumentNullException(nameof(emailTemplateService));
        private readonly IEmailSenderService _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
        private readonly IEmailRepository _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));

        public async Task<ErrorOr<Unit>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Validations

                var user = await _userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);

                if (user == null)
                {
                    return UserErrors.User.UserNotFound(request.UserId);
                }

                var token = await _userTokenRepository.GetAsync(user.Id!, request.Token, cancellationToken);

                if (token == null)
                {
                    return UserTokenErrors.UserToken.TokenNotFound(request.Token);
                }

                if (token.IsExpired)
                {
                    return UserTokenErrors.UserToken.TokenExpired;
                }

                if (token.IsUsed)
                {
                    return UserTokenErrors.UserToken.TokenUsed;
                }

                if (!user.IsLocked)
                {
                    return UserErrors.User.UserAlreadyActivated;
                }

                #endregion

                #region Process

                user.ActivateUser();

                await _userRepository.Update(user);

                var email = await _emailRepository.GetByIdAsync(request.EmailId, cancellationToken);

                if (email != null)
                {
                    email.MarkAsVisualized();

                    await _emailRepository.Update(email);
                }

                token.MarkAsUsed();

                await _userTokenRepository.Update(token);

                var newEmail = Email.Create(
                    destinationEmail: user.Email!,
                    subject: EmailConstants.TitleUserActivated,
                    message: _emailTemplateService.GenerateInformativeEmailActivatedUser(user.UserName!.Value));

                await _emailRepository.AddAsync(newEmail, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var emailResult = await _emailSenderService.SendEmailAsync(newEmail, cancellationToken);

                if (emailResult.IsError)
                {
                    newEmail.UpdateStateFailed();

                    await _emailRepository.Update(newEmail);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return emailResult.Errors;
                }

                await _emailRepository.Update(emailResult.Value);

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