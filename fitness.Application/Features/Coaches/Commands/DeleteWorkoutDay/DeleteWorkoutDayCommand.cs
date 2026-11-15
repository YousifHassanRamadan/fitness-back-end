using MediatR;

namespace fitness.Application.Features.Coaches.WorkoutDays.Commands.DeleteWorkoutDay;

public record DeleteWorkoutDayCommand(
    string UserId,
    int WorkoutDayId
) : IRequest<bool>;