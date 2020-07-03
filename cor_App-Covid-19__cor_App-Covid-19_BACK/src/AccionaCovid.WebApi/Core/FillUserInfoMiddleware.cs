using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Core
{
    public class FillUserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public FillUserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }

    public static class FillUserInfoMiddlewareExtensions
    {
        public static IApplicationBuilder UseFillUserInfoMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FillUserInfoMiddleware>();
        }
    }
}
