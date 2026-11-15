using MediatR;

namespace fitness.Application.Features.Clients.Commands.AddProgressRecord;

public record AddProgressRecordCommand(
    decimal Weight,
    decimal? BodyFatPercentage,
    decimal? MuscleMass,
    string? Notes) : IRequest<int>
{
    public string UserId { get; init; } = string.Empty;
}