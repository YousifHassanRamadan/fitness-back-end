using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteMealPlan;

public record DeleteMealPlanCommand(
    string UserId,
    int MealPlanId
) : IRequest<bool>;