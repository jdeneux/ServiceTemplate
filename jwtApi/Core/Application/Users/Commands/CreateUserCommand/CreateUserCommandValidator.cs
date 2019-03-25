using FluentValidation;
using jwtApi.Infrastructure.Persistence;
using System;
using System.Linq;

namespace jwtApi.Core.Application.Users.Commands.CreateUserCommand
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly DataContext _context;

        public CreateUserCommandValidator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(v => v.FirstName).NotNull().NotEmpty();
            RuleFor(v => v.LastName).NotNull().NotEmpty();
            RuleFor(v => v.Username).NotNull().NotEmpty().Must(BeUnique).WithMessage("Username already exists");
            RuleFor(v => v.Password).NotNull().NotEmpty();
            RuleFor(v => v.Role).IsInEnum();
        }

        private bool BeUnique(string username)
        {
            return !_context.Users.Any(x => x.Username == username);
        }
    }
}
