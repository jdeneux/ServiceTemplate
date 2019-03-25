using FluentValidation;
using jwtApi.Infrastructure.Persistence;
using System;
using System.Linq;

namespace jwtApi.Core.Application.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly DataContext _context;

        public DeleteUserCommandValidator(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            RuleFor(v => v.Id).GreaterThanOrEqualTo(0).Must(Exists).WithMessage(cmd => $"User ID {cmd.Id} doesn't exist" );
        }

        private bool Exists(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }
    }
}
