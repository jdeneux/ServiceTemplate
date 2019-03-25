using jwtApi.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace jwtApi.Presentation.Middlewares
{
    public class DomainErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public DomainErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (DomainException ex)
            {
                // Use standard error
                // https://tools.ietf.org/html/rfc7807
                ctx.Response.StatusCode = 422;

                var problemDetails = new ProblemDetails()
                {
                    Detail = ex.Message,
                    Status = 422,
                    Title = $"{ex.ErrorCode}"
                };

                if (ex.GetType() == typeof(RequestValidationException))
                {
                    problemDetails.Detail = JsonConvert.SerializeObject(((RequestValidationException)ex).Failures);
                }

                ctx.Response.Headers.Add("content-type", "application/json; charset=utf-8");
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
        }
    }
}
