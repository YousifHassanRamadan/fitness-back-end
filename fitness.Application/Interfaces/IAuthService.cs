using fitness.Application.Features.Auth.Commands.Login;
using fitness.Application.Features.Auth.Commands.Login.DTO;
using fitness.Application.Features.Auth.Commands.Register;
using fitness.Application.Features.Auth.Queries.GetCurrentUser.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterCommand command);

        Task<AuthResponse> LoginAsync(LoginCommand command);


        Task<AuthResponse> RefreshTokenAsync(string refreshToken);

        Task LogoutAsync(string refreshToken);

        Task<CurrentUserResponse> GetCurrentUserAsync(string userId);
    }
}
