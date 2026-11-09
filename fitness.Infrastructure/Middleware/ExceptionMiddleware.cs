using fitness.Application.Common.Exceptions;
using fitness.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace fitness.Infrastructure.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");

            var (statusCode, body) = ex switch
            {
                ValidationException validationEx => CreateResponse(
                    HttpStatusCode.BadRequest,
                    "Validation failed.",
                    validationEx.Errors),

                UnauthorizedAccessException => CreateResponse(
                    HttpStatusCode.Unauthorized,
                    "Invalid or unauthorized request."),

                KeyNotFoundException => CreateResponse(
                    HttpStatusCode.NotFound,
                    "The requested resource was not found."),

                _ => CreateResponse(
                    HttpStatusCode.InternalServerError,
                    "An unexpected server error occurred.")
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(
                body,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json);
        }
    }

    private static (HttpStatusCode StatusCode, object Body) CreateResponse(
        HttpStatusCode statusCode,
        string message,
        IEnumerable<string>? errors = null)
    {
        return (
            statusCode,
            ApiResponse<object>.FailureResponse(
                message,
                (int)statusCode,
                errors)
        );
    }
}