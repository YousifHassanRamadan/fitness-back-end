using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachMealPlans;

public record GetCoachMealPlansQuery(string UserId)
    : IRequest<List<CoachMealPlanDto>>;