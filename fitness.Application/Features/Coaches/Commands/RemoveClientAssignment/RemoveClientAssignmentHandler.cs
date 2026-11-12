using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.RemoveClientAssignment;

public class RemoveClientAssignmentHandler
    : IRequestHandler<RemoveClientAssignmentCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public RemoveClientAssignmentHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        RemoveClientAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetClientDetailsAsync(
            request.UserId,
            request.ClientId,
            cancellationToken);

        if (client is null)
            return false;

        client.CoachId = null;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}