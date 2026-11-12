
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateCoachProfile;

public record UpdateCoachProfileCommand(
    string FirstName,
    string LastName,
    string? Bio
) : IRequest<bool>
{
    public string UserId { get; init; } = null!;
}