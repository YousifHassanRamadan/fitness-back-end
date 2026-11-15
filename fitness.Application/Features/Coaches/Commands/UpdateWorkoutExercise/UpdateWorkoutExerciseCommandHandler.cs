using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateWorkoutExercise;

public class UpdateWorkoutExerciseCommandHandler
    : IRequestHandler<UpdateWorkoutExerciseCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public UpdateWorkoutExerciseCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateWorkoutExerciseCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Sets <= 0 || request.Reps <= 0 || request.RestSeconds < 0)
            throw new ArgumentException("Invalid workout exercise values.");

        var workoutExercise =
            await _repository.GetWorkoutExerciseByCoachAsync(
                request.WorkoutExerciseId,
                request.UserId,
                cancellationToken);

        if (workoutExercise == null)
            return false;

        workoutExercise.Sets = request.Sets;
        workoutExercise.Reps = request.Reps;
        workoutExercise.RestSeconds = request.RestSeconds;
        workoutExercise.Order = request.Order;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}