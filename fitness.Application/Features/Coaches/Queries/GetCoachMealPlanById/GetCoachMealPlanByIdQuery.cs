using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachMealPlanById;

public record GetCoachMealPlanByIdQuery(
    string UserId,
    int MealPlanId
) : IRequest<CoachMealPlanDetailsDto?>;