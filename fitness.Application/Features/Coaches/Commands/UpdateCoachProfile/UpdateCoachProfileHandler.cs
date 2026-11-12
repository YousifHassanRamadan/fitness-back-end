
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateCoachProfile;

public class UpdateCoachProfileHandler
    : IRequestHandler<UpdateCoachProfileCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public UpdateCoachProfileHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateCoachProfileCommand request,
        CancellationToken cancellationToken)
    {
        var coach = await _repository.GetByUserIdAsync(
            request.UserId, cancellationToken);

        if (coach is null)
            return false;

        coach.FirstName = request.FirstName;
        coach.LastName = request.LastName;
        coach.Bio = request.Bio;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
