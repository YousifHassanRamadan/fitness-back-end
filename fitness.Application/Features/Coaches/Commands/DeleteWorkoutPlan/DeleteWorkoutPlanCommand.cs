using MediatR;

namespace fitness.Application.Features.Coaches.WorkoutPlans.Commands.DeleteWorkoutPlan;

public record DeleteWorkoutPlanCommand(
    string UserId,
    int WorkoutPlanId
) : IRequest<bool>;