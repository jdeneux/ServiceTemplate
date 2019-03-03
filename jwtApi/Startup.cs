using jwtApi.Helpers;
using jwtApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using AutoMapper;
using jwtApi.Config;
using Microsoft.AspNetCore.Http;

namespace jwtApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("UserDB"));

            services.AddAppMvc();

            services.AddAutoMapper();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAppAuthentication(key);

            // Add HealthCheck support
            services.AddAppHealthCheck();

            // Add Swagger
            services.AddAppSwagger();

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHttpsRedirection();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.When(env.IsDevelopment(), app.UseDeveloperExceptionPage)
                .When(env.IsDevelopment(), app.UseAppSwagger)
                .When(!env.IsDevelopment(), app.UseHsts)
                .UseAppHealthCheck()
                .UseAppAuthentication()
                .UseMiddleware<DomainErrorHandlerMiddleware>()
                .UseAppMvc()
                .Run(NotFoundHandler);
        }

        private readonly RequestDelegate NotFoundHandler =
            async ctx =>
            {
                ctx.Response.StatusCode = 404;
                await ctx.Response.WriteAsync("Page not found.");
            };

    }
}
