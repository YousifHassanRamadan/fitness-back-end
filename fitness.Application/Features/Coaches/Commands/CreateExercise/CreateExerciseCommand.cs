using fitness.Domain.Enums;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.CreateExercise;

public record CreateExerciseCommand : IRequest<int>
{
    public string UserId { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public MuscleGroup MuscleGroup { get; init; }
    public string? Equipment { get; init; }
    public string? VideoUrl { get; init; }
}