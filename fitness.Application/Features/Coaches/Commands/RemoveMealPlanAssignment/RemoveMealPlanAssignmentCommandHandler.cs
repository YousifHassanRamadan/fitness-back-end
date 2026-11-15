using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveMealPlanAssignment;

public class RemoveMealPlanAssignmentCommandHandler
    : IRequestHandler<RemoveMealPlanAssignmentCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public RemoveMealPlanAssignmentCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        RemoveMealPlanAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await _repository.GetMealPlanAssignmentByCoachAsync(
            request.AssignmentId,
            request.ClientId,
            request.UserId,
            cancellationToken);

        if (assignment == null)
            return false;

        await _repository.DeleteMealPlanAssignmentAsync(
            assignment,
            cancellationToken);

        return true;
    }
}