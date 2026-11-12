using MediatR;

namespace fitness.Application.Features.Clients.Commands.UpdateClientProfile;

public record UpdateClientProfileCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Address
) : IRequest<bool>
{
    public string UserId { get; init; } = null!;
}