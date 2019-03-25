using jwtApi.Core.Application.Users.Models;
using AutoMapper;
using jwtApi.Core.Domain.Entities;
using jwtApi.Core.Domain.Exceptions;
using jwtApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace jwtApi.Core.Application.Users.Queries.GetAllUsersQuery
{
    public class GetAllUsersQuery : IRequest<UserViewModel[]>
    {
        public class Handler : IRequestHandler<GetAllUsersQuery, UserViewModel[]>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserViewModel[]> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var entities = await _context.Users.ToListAsync(cancellationToken);

                if (entities == null || entities?.Count == 0)
                {
                    throw new NotFoundException(nameof(User), "All");
                }

                return _mapper.Map<UserViewModel[]>(entities);
            }
        }
    }
}
