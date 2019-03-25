using AutoMapper;
using jwtApi.Core.Application.Interfaces.Mapping;
using jwtApi.Core.Application.Security;
using jwtApi.Core.Application.Users.Models;
using jwtApi.Core.Domain.Entities;
using jwtApi.Infrastructure.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace jwtApi.Core.Application.Users.Commands.CreateUserCommand
{
    public class CreateUserCommand : IMapFrom<User>, IRequest<UserViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public class Handler : IRequestHandler<CreateUserCommand, UserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var newUser = _mapper.Map<User>(request);

                CryptoHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return _mapper.Map<UserViewModel>(newUser);
            }
        }
    }
}
