using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AddWorkoutExercise;

public record AddWorkoutExerciseCommand(
    int WorkoutDayId,
    int ExerciseId,
    int Sets,
    int Reps,
    int RestSeconds,
    int Order
) : IRequest<int>
{
    public string UserId { get; init; } = null!;
}