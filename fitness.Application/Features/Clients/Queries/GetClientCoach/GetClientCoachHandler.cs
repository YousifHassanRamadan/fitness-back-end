using AutoMapper;
using fitness.Application.Features.Clients.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientCoach;

public class GetClientCoachHandler
    : IRequestHandler<GetClientCoachQuery, ClientCoachDto?>
{
    private readonly IClientManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetClientCoachHandler(
        IClientManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ClientCoachDto?> Handle(
        GetClientCoachQuery request,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (client?.Coach is null)
            return null;

        return _mapper.Map<ClientCoachDto>(client.Coach);
    }
}