using FluentValidation;

namespace AuthOn.Application.Users.Commands.Update.RefreshToken
{
    public class RefreshTokenUserCommandValidator : AbstractValidator<RefreshTokenUserCommand>
    {
        public RefreshTokenUserCommandValidator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty()
                .WithName("Refresh Token");
        }
    }
}