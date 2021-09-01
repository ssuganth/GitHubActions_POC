using DotnetCoreSampleApp.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace DotnetCoreSampleApp.Internal
{
    public static class ServiceExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}