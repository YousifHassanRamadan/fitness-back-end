using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AssignMealPlan;

public record AssignMealPlanCommand(
    DateTime StartDate,
    DateTime? EndDate) : IRequest<bool>
{
    public string UserId { get; init; } = string.Empty;
    public int ClientId { get; init; }
    public int MealPlanId { get; init; }
}