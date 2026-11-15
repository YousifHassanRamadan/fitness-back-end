using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AssignWorkoutPlan;

public record AssignWorkoutPlanCommand : IRequest<bool>
{
    public string UserId { get; init; } = null!;
    public int ClientId { get; init; }
    public int WorkoutPlanId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}