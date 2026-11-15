using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateWorkoutExercise;

public record UpdateWorkoutExerciseCommand : IRequest<bool>
{
    public string UserId { get; init; } = null!;
    public int WorkoutExerciseId { get; init; }

    public int Sets { get; init; }
    public int Reps { get; init; }
    public int RestSeconds { get; init; }
    public int Order { get; init; }
}