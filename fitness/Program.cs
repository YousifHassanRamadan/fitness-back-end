
using fitness.Infrastructure.Extentions;
using fitness.Infrastructure.Settings;
using fitness.Infrastructure.Middleware;
using fitness.Application.Extensions;
using fitness.Application.Common.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.OpenApi;

using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// JWT Settings
var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()!;

// Authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };

        // Handle 401 Unauthorized
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.FailureResponse(
                    "Authentication failed.",
                    StatusCodes.Status401Unauthorized,
                    new[]
                    {
                        "Access token is missing or invalid."
                    });

                await context.Response.WriteAsJsonAsync(response);
            }
        };
    });

builder.Services.AddAuthorization();

// OpenAPI Configuration
builder.Services.AddOpenApi(options =>
{
    // Add Bearer Security Scheme
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes =
            new Dictionary<string, IOpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your Access Token."
                }
            };

        return Task.CompletedTask;
    });

    // Apply Bearer Authentication to [Authorize] endpoints
    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        var requiresAuthorization = context.Description
            .ActionDescriptor.EndpointMetadata
            .OfType<IAuthorizeData>()
            .Any();

        if (requiresAuthorization)
        {
            operation.Security ??= [];

            operation.Security.Add(
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(
                        "Bearer", context.Document)] = []
                });
        }

        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto,
    KnownNetworks = { },
    KnownProxies = { }
});

app.MapGet("/debug-scheme", (HttpContext context) =>
    new
    {
        Scheme = context.Request.Scheme,
        ForwardedProto = context.Request.Headers["X-Forwarded-Proto"].ToString()
    });

// Seed Roles
using (var scope = app.Services.CreateScope())
{
    await ServiceCollectionExtentions.SeedRolesAsync(
        scope.ServiceProvider);
}

// HTTP Pipeline
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// OpenAPI & Scalar (Development + Production)
app.MapOpenApi();
app.MapScalarApiReference();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/test", () => "API is working");

app.Run();
