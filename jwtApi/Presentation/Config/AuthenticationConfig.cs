using jwtApi.Core.Application.Users.Queries.GetUserByNameQuery;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace jwtApi.Presentation.Config
{
    public static class AuthenticationConfig
    {
        public static IServiceCollection AddAppAuthentication(this IServiceCollection services, byte[] jwtSecret)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                // Check if the user is still valid
                // return unauthorized if user no longer exists
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();
                        var userName = context.Principal.Identity.Name;

                        var user = await mediator.Send(new GetUserByNameQuery { Username = userName });

                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IApplicationBuilder UseAppAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            return app;
        }
    }
}
