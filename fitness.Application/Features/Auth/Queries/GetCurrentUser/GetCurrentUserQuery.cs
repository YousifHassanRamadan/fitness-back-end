using fitness.Application.Features.Auth.Queries.GetCurrentUser.DTO;
using MediatR;

namespace fitness.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<CurrentUserResponse>
{
    public string UserId { get; set; } = string.Empty;
}