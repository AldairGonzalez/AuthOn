using AuthOn.Application.Services.Interfaces;
using AuthOn.Domain.Entities.TokenTypes;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using AuthOn.Domain.Primitives;
using AuthOn.Shared.Errors.ApplicationErrors;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Update.RefreshToken
{
    internal sealed class RefreshTokenUserCommandHandler(
        IUserTokenRepository userTokenRepository,
        IUnitOfWork unitOfWork,
        ITokenManager tokenManager) : IRequestHandler<RefreshTokenUserCommand, ErrorOr<string>>
    {
        private readonly IUserTokenRepository _userTokenRepository = userTokenRepository ?? throw new ArgumentNullException(nameof(userTokenRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ITokenManager _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));

        public async Task<ErrorOr<string>> Handle(RefreshTokenUserCommand request, CancellationToken cancellationToken)
        {
            #region Validations

            var validationResult = _tokenManager.ValidateRefreshToken(request.RefreshToken);

            if (validationResult.IsError)
                return validationResult.Errors;

            var tokenData = validationResult.Value;

            if (tokenData.HasErrors)
                return tokenData.GetErrors().ToList();

            var userIdResult = tokenData.UserId;

            if (userIdResult.IsError || !userIdResult.Value.HasValue)
                return UserTokenErrors.UserToken.TokenNotFound(request.RefreshToken);

            var userId = userIdResult.Value.Value;
            var userTokens = await _userTokenRepository.FindAllAsync(cancellationToken, userId: new UserId(userId));
            var userRefreshToken = userTokens.FirstOrDefault(ut => ut.Token == request.RefreshToken);

            if (userRefreshToken is null)
                return UserTokenErrors.UserToken.TokenNotFound(request.RefreshToken);

            if (userRefreshToken.IsUsed)
                return UserTokenErrors.UserToken.InvalidatedToken;

            #endregion

            #region Proccess

            var userToken = userTokens.FirstOrDefault(ut => ut.TokenTypeId == TokenType.AccessToken.Id && ut.IsUsed == false);

            if(userToken != null)
            {
                userToken.MarkAsUsed();
                await _userTokenRepository.Update(userToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var accessTokenResult = _tokenManager.GenerateAccessToken(userId);

            if (accessTokenResult.IsError)
                return accessTokenResult.Errors;

            await _userTokenRepository.AddAsync(UserToken.Create(TokenType.AccessToken.Id, userId, accessTokenResult.Value), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return accessTokenResult.Value;

            #endregion
        }
    }
}