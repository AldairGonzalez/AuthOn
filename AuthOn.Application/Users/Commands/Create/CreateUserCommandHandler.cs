using AuthOn.Application.Common.Interfaces;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.Entities.TokenTypes;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using AuthOn.Domain.Primitives;
using AuthOn.Domain.ValueObjects;
using AuthOn.Shared.Constants;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Create
{
    internal sealed class CreateUserCommandHandler(
            IUserRepository userRepository,
            IUserTokenRepository userTokenRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IEmailTemplateService emailTemplateService,
            IEmailSenderService emailSenderService,
            IEmailRepository emailRepository,
            ITokenManager tokenManager) : IRequestHandler<CreateUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository ?? throw new ArgumentNullException(nameof(userTokenRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService ?? throw new ArgumentNullException(nameof(emailTemplateService));
        private readonly IEmailSenderService _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
        private readonly IEmailRepository _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
        private readonly ITokenManager _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));

        public async Task<ErrorOr<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Validations

                if (UserName.Create(request.UserName) is not UserName userName)
                {
                    return UserErrors.User.UserNameWithBadFormat;
                }

                if (EmailAddress.Create(request.Email) is not EmailAddress email)
                {
                    return UserErrors.User.EmailWithBadFormat;
                }

                if (Password.Create(request.Password) is not Password password)
                {
                    return UserErrors.User.PasswordWithBadFormat;
                }

                if (await _userRepository.GetByEmailAsync(email, cancellationToken) is not null)
                {
                    return UserErrors.User.EmailAlreadyExists;
                }

                #endregion

                #region Process

                var user = User.Create(userName, email, _passwordHasher.Hash(password.Value));

                await _userRepository.AddAsync(user, cancellationToken);

                var newEmail = Email.Create(
                    destinationEmail: email,
                    subject: EmailConstants.TitleActivationUser,
                    message: "");

                await _emailRepository.AddAsync(newEmail, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                string token = _tokenManager.GenerateActivationToken(user.Id!.Value, newEmail.Id).Value;

                newEmail.UpdateMessage(_emailTemplateService.GenerateEmailWithActivateUserAction(user.UserName!.Value, token));

                var emailResult = await _emailSenderService.SendEmailAsync(newEmail, cancellationToken);

                if (emailResult.IsError)
                {
                    newEmail.UpdateStateFailed();

                    await _emailRepository.Update(newEmail);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return emailResult.Errors;
                }

                await _emailRepository.Update(emailResult.Value);

                await _userTokenRepository.AddAsync(UserToken.Create(TokenType.ActivationToken.Id, user.Id!.Value, token), cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;

                #endregion
            }
            catch (Exception ex)
            {
                return CommandHandlerErrors.User.UnexpectedErrorWhenCreatingUser(ex.Message);
            }

        }
    }
}