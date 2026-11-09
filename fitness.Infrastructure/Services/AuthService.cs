using fitness.Application.Common.Exceptions;
using fitness.Application.Features.Auth.Commands.Login;
using fitness.Application.Features.Auth.Commands.Login.DTO;
using fitness.Application.Features.Auth.Commands.Register;
using fitness.Application.Features.Auth.Queries.GetCurrentUser.DTO;
using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using fitness.Infrastructure.Identity;
using fitness.Infrastructure.Persistence;
using fitness.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace fitness.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FitnessDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        FitnessDbContext context,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> LoginAsync(LoginCommand command)
    {
        // 1. Find user
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        // 2. Verify password
        var isPasswordValid = await _userManager.CheckPasswordAsync(
            user,
            command.Password);

        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid email or password.");

        // 3. Generate Access Token
        var accessToken = _tokenService.GenerateAccessToken(
            user.Id,
            user.Email!);

        // 4. Generate Refresh Token
        var refreshToken = _tokenService.GenerateRefreshToken();

        // 5. Hash Refresh Token
        var refreshTokenHash = _tokenService.HashRefreshToken(refreshToken);

        // 6. Find existing Refresh Token record
        var existingToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.UserId == user.Id);

        var now = DateTime.UtcNow;

        if (existingToken != null)
        {
            // Replace the old token
            existingToken.TokenHash = refreshTokenHash;
            existingToken.CreatedAt = now;
            existingToken.ExpiresAt = now.AddDays(
                _jwtSettings.RefreshTokenExpirationInDays);
            existingToken.RevokedAt = null;
        }
        else
        {
            // Create a new token record
            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                CreatedAt = now,
                ExpiresAt = now.AddDays(
                    _jwtSettings.RefreshTokenExpirationInDays)
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
        }

        // 7. Save changes
        await _context.SaveChangesAsync();

        // 8. Return response
        return new AuthResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        // 1. Hash the received refresh token
        var tokenHash = _tokenService.HashRefreshToken(refreshToken);

        // 2. Find the token in the database
        var storedToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        // 3. Validate the token
        if (storedToken == null ||
            storedToken.RevokedAt != null ||
            storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException(
                "Invalid or expired refresh token.");
        }

        // 4. Find the user
        var user = await _userManager.FindByIdAsync(storedToken.UserId);

        if (user == null || string.IsNullOrEmpty(user.Email))
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        // 5. Generate new tokens
        var newAccessToken = _tokenService.GenerateAccessToken(
            user.Id,
            user.Email);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var newRefreshTokenHash =
            _tokenService.HashRefreshToken(newRefreshToken);

        // 6. Replace the old refresh token
        var now = DateTime.UtcNow;

        storedToken.TokenHash = newRefreshTokenHash;
        storedToken.CreatedAt = now;
        storedToken.ExpiresAt = now.AddDays(
            _jwtSettings.RefreshTokenExpirationInDays);

        storedToken.RevokedAt = null;

        // 7. Save changes
        await _context.SaveChangesAsync();

        // 8. Return response
        return new AuthResponse
        {
            UserId = user.Id,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<string> RegisterAsync(RegisterCommand command)
    {
        var existingUser = await _userManager.FindByEmailAsync(command.Email);

        if (existingUser != null)
            throw new ValidationException(
                new[] { "Email is already registered." });

        var user = new ApplicationUser
        {
            UserName = command.Email,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        var result = await _userManager.CreateAsync(
            user,
            command.Password);

        if (!result.Succeeded)
        {
            throw new ValidationException(
                result.Errors.Select(e => e.Description));
        }

        try
        {
            var roleResult = await _userManager.AddToRoleAsync(
                user,
                "Client");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ",
                    roleResult.Errors.Select(e => e.Description));

                throw new Exception(errors);
            }

            var client = new Client
            {
                UserId = user.Id,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Gender = command.Gender,
                DateOfBirth = command.DateOfBirth,
                Address = command.Address
            };

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            return user.Id;
        }
        catch
        {
            await _userManager.DeleteAsync(user);
            throw;
        }
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var tokenHash = _tokenService.HashRefreshToken(refreshToken);

        var storedToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        if (storedToken == null || storedToken.RevokedAt != null)
            return;

        storedToken.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<CurrentUserResponse> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException("User not found.");

        var roles = await _userManager.GetRolesAsync(user);

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.UserId == userId);

        var coach = await _context.Coaches
            .FirstOrDefaultAsync(c => c.UserId == userId);

        return new CurrentUserResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            FirstName = client?.FirstName ?? coach?.FirstName,
            LastName = client?.LastName ?? coach?.LastName,
            Roles = roles.ToList()
        };
    }
}