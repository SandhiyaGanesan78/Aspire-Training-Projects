using Microsoft.AspNetCore.Builder;
using CombineSample.Data;

public static class ErrorEmailMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorEmailMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorEmailMiddleware>();
    }
}
