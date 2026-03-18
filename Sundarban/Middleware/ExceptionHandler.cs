using System.Text.Json;
using Sundarban.Exceptions;

namespace Sundarban.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        string message;

        switch (exception)
        {
            case NotFoundException e:
                statusCode = StatusCodes.Status404NotFound;
                message = e.Message;
                break;
            case ValidationException e:
                statusCode = StatusCodes.Status400BadRequest;
                message = e.Message;
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Unexpected error occured";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(response);
    }
}