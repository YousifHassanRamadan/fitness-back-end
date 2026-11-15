using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteExercise;

public class DeleteExerciseCommandHandler
    : IRequestHandler<DeleteExerciseCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public DeleteExerciseCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var exercise = await _repository.GetExerciseByCoachAsync(
            request.ExerciseId,
            request.UserId,
            cancellationToken);

        if (exercise == null)
            return false;

        await _repository.DeleteExerciseAsync(
            exercise,
            cancellationToken);

        return true;
    }
}