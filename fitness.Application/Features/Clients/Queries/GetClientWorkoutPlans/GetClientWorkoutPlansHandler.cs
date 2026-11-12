using AutoMapper;
using fitness.Application.Features.Clients.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientWorkoutPlans;

public class GetClientWorkoutPlansHandler
    : IRequestHandler<GetClientWorkoutPlansQuery, List<WorkoutPlanDto>>
{
    private readonly IClientManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetClientWorkoutPlansHandler(
        IClientManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<WorkoutPlanDto>> Handle(
        GetClientWorkoutPlansQuery request,
        CancellationToken cancellationToken)
    {
        var assignments = await _repository.GetWorkoutPlansAsync(
            request.UserId,
            cancellationToken);

        return _mapper.Map<List<WorkoutPlanDto>>(assignments);
    }
}