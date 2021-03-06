﻿using FluentValidation.AspNetCore;
using jwtApi.Core.Application.Users.Queries.GetUserByIdQuery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace jwtApi.Presentation.Config
{
    public static class MvcConfig
    {
        public static IServiceCollection AddAppMvc(this IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetUserByIdQueryValidator>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }

        public static IApplicationBuilder UseAppMvc(this IApplicationBuilder app)
        {
            app.UseMvc();
            return app;
        }
    }
}
