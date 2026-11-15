using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveWorkoutExercise;

public record RemoveWorkoutExerciseCommand(
    string UserId,
    int WorkoutExerciseId
) : IRequest<bool>;