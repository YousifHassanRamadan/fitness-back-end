using MediatR;

namespace fitness.Application.Features.Coaches.Commands.CreateWorkoutPlan;

public record CreateWorkoutPlanCommand(
    string Name,
    string? Description,
    int DurationInWeeks
) : IRequest<int>
{
    public string UserId { get; init; } = null!;
}