using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AssignClient;

public class AssignClientHandler
    : IRequestHandler<AssignClientCommand, AssignClientResult>
{
    private readonly ICoachManagementRepository _repository;

    public AssignClientHandler(ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<AssignClientResult> Handle(
        AssignClientCommand request,
        CancellationToken cancellationToken)
    {
        var coach = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (coach is null)
            return AssignClientResult.CoachNotFound;

        var client = await _repository.GetClientByIdAsync(
            request.ClientId,
            cancellationToken);

        if (client is null)
            return AssignClientResult.ClientNotFound;

        if (client.CoachId == coach.Id)
            return AssignClientResult.AlreadyAssignedToThisCoach;

        if (client.CoachId is not null)
            return AssignClientResult.AssignedToAnotherCoach;

        client.CoachId = coach.Id;

        await _repository.SaveChangesAsync(cancellationToken);

        return AssignClientResult.Success;
    }
}