using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.WorkoutPlans.Commands.DeleteWorkoutPlan;

public class DeleteWorkoutPlanCommandHandler
    : IRequestHandler<DeleteWorkoutPlanCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public DeleteWorkoutPlanCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteWorkoutPlanCommand request,
        CancellationToken cancellationToken)
    {
        var workoutPlan = await _repository.GetWorkoutPlanByCoachAsync(
            request.WorkoutPlanId,
            request.UserId,
            cancellationToken);

        if (workoutPlan is null)
            return false;

        await _repository.DeleteWorkoutPlanAsync(
            workoutPlan,
            cancellationToken);

        return true;
    }
}