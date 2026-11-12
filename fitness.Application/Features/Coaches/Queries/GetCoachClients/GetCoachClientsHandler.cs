using AutoMapper;
using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachClients;

public class GetCoachClientsHandler
    : IRequestHandler<GetCoachClientsQuery, List<CoachClientDto>?>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetCoachClientsHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CoachClientDto>?> Handle(
        GetCoachClientsQuery request,
        CancellationToken cancellationToken)
    {
        var coach = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (coach is null)
            return null;

        var clients = await _repository.GetClientsAsync(
            request.UserId,
            cancellationToken);

        return _mapper.Map<List<CoachClientDto>>(clients);
    }
}