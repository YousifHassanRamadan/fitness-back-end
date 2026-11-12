using AutoMapper;
using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachClientDetails;

public class GetCoachClientDetailsHandler
    : IRequestHandler<GetCoachClientDetailsQuery, CoachClientDto?>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetCoachClientDetailsHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CoachClientDto?> Handle(
        GetCoachClientDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetClientDetailsAsync(
            request.UserId,
            request.ClientId,
            cancellationToken);

        return client is null
            ? null
            : _mapper.Map<CoachClientDto>(client);
    }
}