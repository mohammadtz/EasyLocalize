using EasyLocalize.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EasyLocalize.DependencyInjection.Middlewares;
// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class EasyLocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public EasyLocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        var messages = httpContext.RequestServices.GetService(typeof(IMessage)) as IMessage;
        if(messages == null) return _next(httpContext);
        
        httpContext.Request.Headers.TryGetValue(messages.DefaultHeaderName, out var acceptedLanguage);
        messages.SetLanguage(acceptedLanguage);
        return _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseEasyLocalization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<EasyLocalizationMiddleware>();
    }
}