using FluentValidation;

namespace AuthOn.Application.Entities.Emails.Commands.Create
{
    class CreateEmailCommandValidator : AbstractValidator<CreateEmailCommand>
    {
        public CreateEmailCommandValidator() 
        {
            RuleFor(r => r.DestinationEmail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255)
                .WithName("Destination Email");
        }
    }
}