using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveWorkoutExercise;

public class RemoveWorkoutExerciseCommandHandler
    : IRequestHandler<RemoveWorkoutExerciseCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public RemoveWorkoutExerciseCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        RemoveWorkoutExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var workoutExercise =
            await _repository.GetWorkoutExerciseByCoachAsync(
                request.WorkoutExerciseId,
                request.UserId,
                cancellationToken);

        if (workoutExercise == null)
            return false;

        await _repository.DeleteWorkoutExerciseAsync(
            workoutExercise,
            cancellationToken);

        return true;
    }
}