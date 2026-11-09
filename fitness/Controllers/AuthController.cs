using fitness.Application.Features.Auth.Commands.Login;
using fitness.Application.Features.Auth.Commands.Logout;
using fitness.Application.Features.Auth.Commands.RefreshToken;
using fitness.Application.Features.Auth.Commands.Register;
using fitness.Application.Features.Auth.Queries.GetCurrentUser;
using fitness.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace fitness.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtSettings _jwtSettings;

    public AuthController(IMediator mediator, IOptions<JwtSettings> jwtSettings)
    {
        _mediator = mediator;
        _jwtSettings = jwtSettings.Value;

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);

        return Ok(new { userId });
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login(
    LoginCommand command,
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        Response.Cookies.Append("refreshToken", response.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        return Ok(new
        {
            response.AccessToken,
            response.UserId
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken)
            || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Refresh token is missing." });
        }

        var command = new RefreshTokenCommand
        {
            RefreshToken = refreshToken
        };

        var response = await _mediator.Send(command, cancellationToken);

        Response.Cookies.Append(
            "refreshToken",
            response.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpirationInDays)
            });

        return Ok(new
        {
            response.AccessToken,
            response.UserId
        });
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken)
            && !string.IsNullOrEmpty(refreshToken))
        {
            var command = new LogoutCommand
            {
                RefreshToken = refreshToken
            };

            await _mediator.Send(command, cancellationToken);
        }

        Response.Cookies.Delete(
            "refreshToken",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(
    CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var query = new GetCurrentUserQuery
        {
            UserId = userId
        };

        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}



