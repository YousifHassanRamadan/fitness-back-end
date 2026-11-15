using fitness.Domain.Enums;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateExercise;

public record UpdateExerciseCommand : IRequest<bool>
{
    public string UserId { get; init; } = null!;
    public int ExerciseId { get; init; }

    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public MuscleGroup MuscleGroup { get; init; }
    public string? Equipment { get; init; }
    public string? VideoUrl { get; init; }
}