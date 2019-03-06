using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;

namespace jwtApi.Config
{
    public static class SwaggerConfig
    {
        private static string Endpoint => $"/swagger/{AppConfig.Version}/swagger.json";
        private static string UiEndpoint => $"";

        public static IServiceCollection AddAppSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(_swaggerGenConfig);
            return services;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger().
                UseSwaggerUI(_swaggerUIConfig);
            return app;
        }

        private static void _swaggerUIConfig(SwaggerUIOptions config)
        {
            config.RoutePrefix = UiEndpoint;
            config.SwaggerEndpoint(Endpoint, AppConfig.Name);
        }

        private static void _swaggerGenConfig(SwaggerGenOptions config)
        {
            config.SwaggerDoc(
                AppConfig.Version,
                new Info { Version = AppConfig.Version, Title = AppConfig.Name });

            config.DescribeAllEnumsAsStrings();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            config.IncludeXmlComments(xmlPath);
        }
    }
}
