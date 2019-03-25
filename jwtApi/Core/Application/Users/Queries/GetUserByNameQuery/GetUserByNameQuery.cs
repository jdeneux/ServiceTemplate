using AutoMapper;
using jwtApi.Core.Application.Users.Models;
using jwtApi.Core.Domain.Entities;
using jwtApi.Core.Domain.Exceptions;
using jwtApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace jwtApi.Core.Application.Users.Queries.GetUserByNameQuery
{
    public class GetUserByNameQuery : IRequest<UserViewModel>
    {
        public string Username { get; set; }

        public class Handler : IRequestHandler<GetUserByNameQuery, UserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserViewModel> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(User), request.Username);
                }

                return _mapper.Map<UserViewModel>(entity);
            }
        }
    }
}
