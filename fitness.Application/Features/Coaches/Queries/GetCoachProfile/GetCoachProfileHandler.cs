using AutoMapper;
using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachProfile;

public class GetCoachProfileHandler
    : IRequestHandler<GetCoachProfileQuery, CoachProfileDto?>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetCoachProfileHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CoachProfileDto?> Handle(
        GetCoachProfileQuery request,
        CancellationToken cancellationToken)
    {
        var coach = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        return coach is null
            ? null
            : _mapper.Map<CoachProfileDto>(coach);
    }
}