using FluentValidation;
using jwtApi.Infrastructure.Persistence;
using System;
using System.Linq;

namespace jwtApi.Core.Application.Users.Commands.UpdateUserCommand
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly DataContext _context;

        public UpdateUserCommandValidator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(v => v.Id).GreaterThanOrEqualTo(0).Must(Exists).WithMessage(cmd => $"User ID {cmd.Id} doesn't exist" );
            RuleFor(v => v.FirstName).NotNull().NotEmpty();
            RuleFor(v => v.LastName).NotNull().NotEmpty();
            RuleFor(v => v.Username).NotNull().NotEmpty();
            RuleFor(v => v.Role).IsInEnum();
        }

        private bool Exists(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }
    }
}
