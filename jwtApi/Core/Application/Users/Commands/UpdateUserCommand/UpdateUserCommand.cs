using AutoMapper;
using jwtApi.Core.Application.Interfaces.Mapping;
using jwtApi.Core.Application.Security;
using jwtApi.Core.Application.Users.Models;
using jwtApi.Core.Domain.Entities;
using jwtApi.Core.Domain.Exceptions;
using jwtApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace jwtApi.Core.Application.Users.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : IMapFrom<User>, IRequest<UserViewModel>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public class Handler : IRequestHandler<UpdateUserCommand, UserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserViewModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (request.Username != user.Username)
                {
                    // username has changed so check if the new username is already taken
                    if (_context.Users.Any(x => x.Username == request.Username))
                        throw new DomainException($"Username {request.Username} is already taken");
                }

                // update user properties
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Username = request.Username;

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    CryptoHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                _context.Users.Update(user);
                _context.SaveChanges();

                return _mapper.Map<UserViewModel>(user);
            }
        }
    }
}
