using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AddWorkoutDay;

public record AddWorkoutDayCommand(
    int WorkoutPlanId,
    int DayNumber,
    string Name
) : IRequest<int>
{
    public string UserId { get; init; } = null!;
}