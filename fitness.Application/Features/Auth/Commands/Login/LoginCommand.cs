using fitness.Application.Features.Auth.Commands.Login.DTO;
using MediatR;

namespace fitness.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}