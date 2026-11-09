using fitness.Application.Features.Auth.Queries.GetCurrentUser.DTO;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    private readonly IAuthService _authService;

    public GetCurrentUserQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<CurrentUserResponse> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        return await _authService.GetCurrentUserAsync(request.UserId);
    }
}