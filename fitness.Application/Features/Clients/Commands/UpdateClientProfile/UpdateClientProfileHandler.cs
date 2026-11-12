using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Clients.Commands.UpdateClientProfile;

public class UpdateClientProfileHandler
    : IRequestHandler<UpdateClientProfileCommand, bool>
{
    private readonly IClientManagementRepository _repository;

    public UpdateClientProfileHandler(
        IClientManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateClientProfileCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (client is null)
            return false;

        client.FirstName = request.FirstName;
        client.LastName = request.LastName;
        client.DateOfBirth = request.DateOfBirth;
        client.Address = request.Address;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}