using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveWorkoutPlanAssignment;

public class RemoveWorkoutPlanAssignmentCommandHandler
    : IRequestHandler<RemoveWorkoutPlanAssignmentCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public RemoveWorkoutPlanAssignmentCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        RemoveWorkoutPlanAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignment =
            await _repository.GetWorkoutPlanAssignmentByCoachAsync(
                request.AssignmentId,
                request.ClientId,
                request.UserId,
                cancellationToken);

        if (assignment == null)
            return false;

        await _repository.DeleteWorkoutPlanAssignmentAsync(
            assignment,
            cancellationToken);

        return true;
    }
}