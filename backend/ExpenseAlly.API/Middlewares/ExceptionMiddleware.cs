using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace ExpenseAlly.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ResponseDto { Success = false };
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = (int)unauthorizedException.StatusCode;
                response.Errors = new List<ErrorDto>
                {
                    new ErrorDto
                    {
                        Code = "Unauthorized",
                        Message = unauthorizedException.Message
                    }
                };
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Errors = new List<ErrorDto>
                {
                    new ErrorDto
                    {
                        Code = "ServerError",
                        Message = "An unexpected error occurred.",
                        Details = _env.IsDevelopment() ? exception.StackTrace : null
                    }
                };
                break;
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
