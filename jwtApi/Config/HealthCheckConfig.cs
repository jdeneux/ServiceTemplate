using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace jwtApi.Config
{
    public static class HealthCheckConfig
    {
        public static string EndPoint = "/health";

        public static IServiceCollection AddAppHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks();
            return services;
        }

        public static IApplicationBuilder UseAppHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks(EndPoint);
            return app;
        }
    }
}
