using AuthOn.Application.Common.Interfaces;
using AuthOn.Domain.DomainErrors;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Primitives;
using AuthOn.Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace AuthOn.Application.Users.Commands.Create
{
    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<ErrorOr<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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

            var user = User.Create(userName, email, _passwordHasher.Hash(password.Value));

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}