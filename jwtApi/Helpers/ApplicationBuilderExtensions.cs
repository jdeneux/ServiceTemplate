using Microsoft.AspNetCore.Builder;
using System;

namespace jwtApi.Helpers
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder When(
            this IApplicationBuilder builder,
            bool predicate,
            Func<IApplicationBuilder> compose) => predicate ? compose() : builder;
    }
}
