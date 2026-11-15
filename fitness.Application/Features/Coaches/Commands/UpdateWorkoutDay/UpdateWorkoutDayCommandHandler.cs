using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.WorkoutDays.Commands.UpdateWorkoutDay;

public class UpdateWorkoutDayCommandHandler
    : IRequestHandler<UpdateWorkoutDayCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public UpdateWorkoutDayCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateWorkoutDayCommand request,
        CancellationToken cancellationToken)
    {
        var workoutDay = await _repository.GetWorkoutDayByCoachAsync(
            request.WorkoutDayId,
            request.UserId,
            cancellationToken);

        if (workoutDay is null)
            return false;

        if (request.DayNumber <= 0 || string.IsNullOrWhiteSpace(request.Name))
            return false;

        workoutDay.DayNumber = request.DayNumber;
        workoutDay.Name = request.Name;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}