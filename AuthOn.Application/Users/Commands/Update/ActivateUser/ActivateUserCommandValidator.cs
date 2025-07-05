using FluentValidation;

namespace AuthOn.Application.Users.Commands.Update.ActivateUser
{
    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithName("User ID");

            RuleFor(x => x.EmailId)
                .NotEmpty()
                .WithName("Email ID");

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithName("Token");
        }
    }
}