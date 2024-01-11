using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class ErrorEmailMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorEmailMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log or send email with error details
            await SendErrorEmail(ex, context);

            // Rethrow the exception to be handled by the default exception handler
            throw;
        }
    }

    private async Task SendErrorEmail(Exception ex, HttpContext context)
    {
        // Your code to send the error email
        // Access the necessary data from the HttpContext and ex objects
        // Send the email using your email service or library
    }
}
