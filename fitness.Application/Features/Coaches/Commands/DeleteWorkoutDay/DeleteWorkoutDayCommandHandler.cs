using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.WorkoutDays.Commands.DeleteWorkoutDay;

public class DeleteWorkoutDayCommandHandler
    : IRequestHandler<DeleteWorkoutDayCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public DeleteWorkoutDayCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteWorkoutDayCommand request,
        CancellationToken cancellationToken)
    {
        var workoutDay = await _repository.GetWorkoutDayByCoachAsync(
            request.WorkoutDayId,
            request.UserId,
            cancellationToken);

        if (workoutDay is null)
            return false;

        await _repository.DeleteWorkoutDayAsync(
            workoutDay,
            cancellationToken);

        return true;
    }
}