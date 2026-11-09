using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request.RefreshToken);

        return Unit.Value;
    }
}