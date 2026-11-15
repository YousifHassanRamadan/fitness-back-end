using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateWorkoutPlan;

public class UpdateWorkoutPlanCommandHandler
    : IRequestHandler<UpdateWorkoutPlanCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public UpdateWorkoutPlanCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateWorkoutPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _repository.GetWorkoutPlanByCoachAsync(
            request.WorkoutPlanId,
            request.UserId,
            cancellationToken);

        if (plan is null)
            return false;

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Workout plan name is required.");

        if (request.DurationInWeeks <= 0)
            throw new ArgumentException(
                "Duration must be greater than zero.");

        plan.Name = request.Name;
        plan.Description = request.Description;
        plan.DurationInWeeks = request.DurationInWeeks;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}