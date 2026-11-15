using MediatR;

namespace fitness.Application.Features.Coaches.WorkoutDays.Commands.UpdateWorkoutDay;

public record UpdateWorkoutDayCommand : IRequest<bool>
{
    public string UserId { get; init; } = null!;
    public int WorkoutDayId { get; init; }
    public int DayNumber { get; init; }
    public string Name { get; init; } = null!;
}