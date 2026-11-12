using fitness.Application.Features.Clients.DTOs;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientMealPlans;

public record GetClientMealPlansQuery(string UserId)
    : IRequest<List<MealPlanDto>>;