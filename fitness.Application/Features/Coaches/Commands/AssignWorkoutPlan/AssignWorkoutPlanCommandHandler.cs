using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AssignWorkoutPlan;

public class AssignWorkoutPlanCommandHandler
    : IRequestHandler<AssignWorkoutPlanCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public AssignWorkoutPlanCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        AssignWorkoutPlanCommand request,
        CancellationToken cancellationToken)
    {
        if (request.StartDate == default ||
            (request.EndDate.HasValue &&
             request.EndDate.Value < request.StartDate))
        {
            throw new ArgumentException("Invalid assignment dates.");
        }

        var client = await _repository.GetClientByIdAsync(
            request.ClientId,
            cancellationToken);

        if (client == null || client.Coach?.UserId != request.UserId)
            return false;

        var workoutPlan = await _repository.GetWorkoutPlanByCoachAsync(
            request.WorkoutPlanId,
            request.UserId,
            cancellationToken);

        if (workoutPlan == null)
            return false;

        var assignment = new WorkoutPlanAssignment
        {
            ClientId = client.Id,
            WorkoutPlanId = workoutPlan.Id,
            AssignedAt = DateTime.UtcNow,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _repository.AddWorkoutPlanAssignmentAsync(
            assignment,
            cancellationToken);

        return true;
    }
}