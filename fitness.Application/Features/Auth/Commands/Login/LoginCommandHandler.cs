using fitness.Application.Features.Auth;
using fitness.Application.Features.Auth.Commands.Login.DTO;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request);
    }
}