using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateWorkoutPlan;

public record UpdateWorkoutPlanCommand : IRequest<bool>
{
    public string UserId { get; init; } = null!;
    public int WorkoutPlanId { get; init; }

    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int DurationInWeeks { get; init; }
}