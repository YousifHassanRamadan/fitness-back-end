using fitness.Application.Features.Auth.Commands.Login.DTO;
using MediatR;

namespace fitness.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}