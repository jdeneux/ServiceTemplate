using CondenserDotNet.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace jwtApi.Config
{
    public static class CondenserConfig
    {
        public static IServiceCollection AddAppConsulServices(this IServiceCollection services)
        {
            services.AddConsulServices();

            //Setup Condenser config configuration
            services.Configure<ServiceManagerConfig>(config =>
            {
                config.ServiceName = AppConfig.Name;
                config.ServiceId = $"{config.ServiceName}:{Dns.GetHostName()}";
            });

            return services;
        }

        public static IServiceManager UseAppConsulServices(this IServiceManager serviceManager)
        {
            //Register the service in consul
            serviceManager.
                AddHttpHealthCheck(HealthCheckConfig.EndPoint, 15).
                WithDeregisterIfCriticalAfterMinutes(5).
                RegisterServiceAsync();

            return serviceManager;
        }
    }
}
