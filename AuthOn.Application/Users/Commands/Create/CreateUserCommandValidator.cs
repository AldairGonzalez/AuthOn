using FluentValidation;

namespace AuthOn.Application.Users.Commands.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(20)
                .WithName("User Name");

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