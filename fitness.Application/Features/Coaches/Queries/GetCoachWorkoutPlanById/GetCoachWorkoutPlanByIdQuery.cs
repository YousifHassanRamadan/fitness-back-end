using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachWorkoutPlanById;

public record GetCoachWorkoutPlanByIdQuery(
    string UserId,
    int WorkoutPlanId
) : IRequest<WorkoutPlanDetailsDto?>;