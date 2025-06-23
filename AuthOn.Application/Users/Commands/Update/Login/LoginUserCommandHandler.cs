using AuthOn.Application.Common.Interfaces;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.TokenTypes;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using AuthOn.Domain.Primitives;
using AuthOn.Domain.ValueObjects;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.Login
{
    internal sealed class LoginUserCommandHandler(
        IUserRepository userRepository,
        IUserTokenRepository userTokenRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenManager tokenManager) : IRequestHandler<LoginUserCommand, ErrorOr<LoginUserCommandResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository ?? throw new ArgumentNullException(nameof(userTokenRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private readonly ITokenManager _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));

        public async Task<ErrorOr<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
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

            var accessToken = _tokenManager.GenerateAccessToken(user.Id!.Value);
            var refreshToken = _tokenManager.GenerateRefreshToken(user.Id!.Value);

            if (accessToken.IsError)
                return accessToken.Errors;

            if (refreshToken.IsError)
                return refreshToken.Errors;

            var userTokens = await _userTokenRepository.FindAllAsync(cancellationToken, userId: user.Id);

            if (userTokens is not null)
            {
                foreach(var token in userTokens.Where(x => (x.TokenTypeId == TokenType.RefreshToken.Id || x.TokenTypeId == TokenType.AccessToken.Id) && x.IsUsed == false))
                {
                    token.MarkAsUsed();
                    await _userTokenRepository.Update(token);
                }
            }

            await _userTokenRepository.AddAsync(UserToken.Create(TokenType.AccessToken.Id, user.Id!.Value, accessToken.Value), cancellationToken);
            await _userTokenRepository.AddAsync(UserToken.Create(TokenType.RefreshToken.Id, user.Id!.Value, refreshToken.Value), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new LoginUserCommandResponse()
            {
                AccessToken = accessToken.Value,
                RefreshToken = refreshToken.Value,
                RefreshTokenExpiresInHours = _tokenManager.RefreshTokenExpireInHours.Value,
            };

            return response;

            #endregion
        }
    }
}