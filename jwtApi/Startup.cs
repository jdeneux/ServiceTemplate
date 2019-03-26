using jwtApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using CondenserDotNet.Client;
using jwtApi.Infrastructure.Persistence;
using jwtApi.Presentation.Config;
using jwtApi.Core.Application;
using jwtApi.Presentation.Middlewares;
using MediatR;
using System.Reflection;
using jwtApi.Core.Application.Infrastructure.AutoMapper;
using MediatR.Pipeline;
using jwtApi.Core.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;

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

            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR();

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

            // Add Condenser
            services.AddAppConsulServices();

            // Add Api Version Header management
            services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-version"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceManager serviceManager)
        {
            app.UseHttpsRedirection();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // Register Consul Service
            serviceManager.UseAppConsulServices();

            app.When(env.IsDevelopment(), app.UseDeveloperExceptionPage)
                .When(env.IsDevelopment(), app.UseAppSwagger)
                .When(!env.IsDevelopment(), app.UseHsts)
                .UseAppHealthCheck()
                .UseMiddleware<DomainErrorHandlerMiddleware>()
                .UseAppAuthentication()
                .UseAppMvc()
                .Run(_notFoundHandler); // Default handler for all requests not processed by a Middleware
        }

        private readonly RequestDelegate _notFoundHandler =
            async ctx =>
            {
                ctx.Response.StatusCode = 404;
                await ctx.Response.WriteAsync("Page not found.");
            };

    }
}
