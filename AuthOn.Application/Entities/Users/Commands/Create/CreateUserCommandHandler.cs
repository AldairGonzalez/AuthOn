using AuthOn.Application.Common.Interfaces;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Primitives;
using AuthOn.Domain.ValueObjects;
using AuthOn.Shared.Constants;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Entities.Users.Commands.Create
{
    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailRepository _emailRepository;

        public CreateUserCommandHandler(
            IUserRepository userRepository, 
            IUnitOfWork unitOfWork, 
            IPasswordHasher passwordHasher, 
            IEmailTemplateService emailTemplateService,
            IEmailSenderService emailSenderService,
            IEmailRepository emailRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _emailTemplateService = emailTemplateService ?? throw new ArgumentNullException(nameof(emailTemplateService));
            _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
            _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
        }

        public async Task<ErrorOr<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
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

                if (await _userRepository.GetByEmailAsync(email) is not null)
                {
                    return UserErrors.User.EmailAlreadyExists;
                }

                var user = User.Create(userName, email, _passwordHasher.Hash(password.Value));

                await _userRepository.AddAsync(user);

                var newEmail = Email.Create(
                    email,
                    EmailConstants.TitleActivationEmail,
                    _emailTemplateService.GenerateActivationEmail(user.Id.Value, user.UserName.Value));

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
            }
            catch (Exception ex)
            {
                return CommandHandlerErrors.User.UnexpectedErrorWhenCreatingAUser(ex.Message);
            }

        }
    }
}