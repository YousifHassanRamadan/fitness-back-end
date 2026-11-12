using AutoMapper;
using fitness.Application.Features.Clients.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientProfile;

public class GetClientProfileHandler
    : IRequestHandler<GetClientProfileQuery, ClientProfileDto?>
{
    private readonly IClientManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetClientProfileHandler(
        IClientManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ClientProfileDto?> Handle(
        GetClientProfileQuery request,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        return client is null
            ? null
            : _mapper.Map<ClientProfileDto>(client);
    }
}