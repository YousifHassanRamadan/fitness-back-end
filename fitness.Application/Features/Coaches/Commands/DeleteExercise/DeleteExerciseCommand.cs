using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteExercise;

public record DeleteExerciseCommand(
    string UserId,
    int ExerciseId
) : IRequest<bool>;