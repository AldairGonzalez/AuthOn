using AuthOn.Application.Common.Interfaces;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Primitives;
using AuthOn.Domain.ValueObjects;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Login
{
    internal sealed class LoginUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenManager tokenManager) : IRequestHandler<LoginUserCommand, ErrorOr<string>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private readonly ITokenManager _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));

        public async Task<ErrorOr<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            #region Validations

            if (EmailAddress.Create(request.Email) is not EmailAddress email)
            {
                return UserErrors.User.EmailWithBadFormat;
            }

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return UserErrors.User.UserNotFound(request.Email);
            }

            if (!user.EmailConfirmed)
            {
                return UserErrors.User.EmailAlreadyActivated;
            }

            if (!_passwordHasher.Verify(user.HashedPassword, request.Password))
            {
                if (user.IsLocked)
                {
                    return UserErrors.User.UserIsLocked;
                }
                else
                {
                    user.IncreaseAuthenticationAttempts();

                    await _userRepository.Update(user);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return UserErrors.User.IncorrectPassword;
                }
            }
            else
            {
                if (user.DeletionDate is not null)
                {
                    return UserErrors.User.UserWithActiveDeletionProcess;
                }

                if (user.IsLocked)
                {
                    return UserErrors.User.UserAlreadyActivated;
                }
            }

            #endregion

            #region Process

            user.ResetAuthenticationAttempts();

            await _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var accessToken = _tokenManager.GenerateAccessToken(user.Id!.Value);

            if (accessToken.IsError)
                return accessToken.Errors;

            return accessToken.Value;

            #endregion
        }
    }
}