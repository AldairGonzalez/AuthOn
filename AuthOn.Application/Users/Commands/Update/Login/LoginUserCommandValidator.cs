using FluentValidation;

namespace AuthOn.Application.Users.Commands.Update.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255)
                .WithName("Email Address");
        
            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithName("Password");
        }
    }
}