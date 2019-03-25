using AutoMapper;
using jwtApi.Core.Application.Interfaces.Mapping;
using jwtApi.Core.Domain.Entities;
using jwtApi.Infrastructure.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace jwtApi.Core.Application.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : IMapFrom<User>, IRequest
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteUserCommand>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.Id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
