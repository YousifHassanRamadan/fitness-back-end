using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachWorkoutPlans;

public record GetCoachWorkoutPlansQuery(string UserId)
    : IRequest<List<WorkoutPlanDto>>;