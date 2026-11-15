using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateExercise;

public class UpdateExerciseCommandHandler
    : IRequestHandler<UpdateExerciseCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public UpdateExerciseCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Exercise name is required.");

        var exercise = await _repository.GetExerciseByCoachAsync(
            request.ExerciseId,
            request.UserId,
            cancellationToken);

        if (exercise == null)
            return false;

        exercise.Name = request.Name;
        exercise.Description = request.Description;
        exercise.MuscleGroup = request.MuscleGroup;
        exercise.Equipment = request.Equipment;
        exercise.VideoUrl = request.VideoUrl;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}