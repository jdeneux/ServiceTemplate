using FluentValidation;
using jwtApi.Infrastructure.Persistence;
using System.Linq;

namespace jwtApi.Core.Application.Users.Commands.AuthenticateUserCommand
{
    public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
    {
        private readonly DataContext _context;

        public AuthenticateUserCommandValidator(DataContext context)
        {
            _context = context;

            RuleFor(v => v.Username).NotNull().NotEmpty().Must(Exists).WithMessage("Username doesn't exist");
            RuleFor(v => v.Password).NotNull().NotEmpty();
        }

        private bool Exists(string username)
        {
            return _context.Users.Any(x => x.Username == username);
        }
    }
}
