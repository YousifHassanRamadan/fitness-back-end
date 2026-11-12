using fitness.Application.Features.Clients.DTOs;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientWorkoutPlans;

public record GetClientWorkoutPlansQuery(string UserId)
    : IRequest<List<WorkoutPlanDto>>;