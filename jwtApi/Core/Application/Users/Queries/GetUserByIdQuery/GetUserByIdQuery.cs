using AutoMapper;
using jwtApi.Core.Application.Users.Models;
using jwtApi.Core.Domain.Entities;
using jwtApi.Core.Domain.Exceptions;
using jwtApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace jwtApi.Core.Application.Users.Queries.GetUserByIdQuery
{
    public class GetUserByIdQuery : IRequest<UserViewModel>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetUserByIdQuery, UserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(User), request.Id);
                }

                return _mapper.Map<UserViewModel>(entity);
            }
        }
    }
}
