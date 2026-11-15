using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Clients.Commands.AddProgressRecord;

public class AddProgressRecordCommandHandler
    : IRequestHandler<AddProgressRecordCommand, int>
{
    private readonly IClientManagementRepository _repository;

    public AddProgressRecordCommandHandler(
        IClientManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        AddProgressRecordCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (client == null)
            return 0;

        var progressRecord = new ProgressRecord
        {
            ClientId = client.Id,
            Weight = request.Weight,
            BodyFatPercentage = request.BodyFatPercentage,
            MuscleMass = request.MuscleMass,
            RecordedAt = DateTime.UtcNow,
            Notes = request.Notes
        };

        await _repository.AddProgressRecordAsync(
            progressRecord,
            cancellationToken);

        return progressRecord.Id;
    }
}