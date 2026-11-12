using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AddWorkoutDay;

public class AddWorkoutDayHandler
    : IRequestHandler<AddWorkoutDayCommand, int>
{
    private readonly ICoachManagementRepository _repository;

    public AddWorkoutDayHandler(ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        AddWorkoutDayCommand request,
        CancellationToken cancellationToken)
    {
        var workoutPlan = await _repository.GetWorkoutPlanByCoachAsync(
            request.WorkoutPlanId,
            request.UserId,
            cancellationToken);

        if (workoutPlan is null)
            throw new KeyNotFoundException("Workout plan not found.");

        var workoutDay = new WorkoutDay
        {
            WorkoutPlanId = workoutPlan.Id,
            DayNumber = request.DayNumber,
            Name = request.Name
        };

        await _repository.AddWorkoutDayAsync(
            workoutDay,
            cancellationToken);

        return workoutDay.Id;
    }
}