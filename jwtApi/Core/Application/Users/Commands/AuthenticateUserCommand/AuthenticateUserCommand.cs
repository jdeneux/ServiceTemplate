using AutoMapper;
using jwtApi.Core.Application.Security;
using jwtApi.Core.Domain.Entities;
using jwtApi.Core.Domain.Exceptions;
using jwtApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jwtApi.Core.Application.Users.Commands.AuthenticateUserCommand
{
    public class AuthenticateUserCommand : IRequest<AuthenticateUserViewModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserViewModel>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly AppSettings _appSettings;

            public Handler(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
            {
                _context = context;
                _mapper = mapper;
                _appSettings = appSettings.Value;
            }

            public async Task<AuthenticateUserViewModel> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

                // check if password is correct
                if (!CryptoHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    throw new AuthenticationException();

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                //Setup Claim Identity
                var claim = new Claim(ClaimTypes.Name, user.Username);
                claim.Properties.Add(ClaimTypes.Role, Enum.GetName(typeof(Role), user.Role));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] { claim }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                token.Payload["role"] = Enum.GetName(typeof(Role), user.Role);
                user.Token = tokenHandler.WriteToken(token);

                return _mapper.Map<AuthenticateUserViewModel>(user);
            }
        }
    }
}
