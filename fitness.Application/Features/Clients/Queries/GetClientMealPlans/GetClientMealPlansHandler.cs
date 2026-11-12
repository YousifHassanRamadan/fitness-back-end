using AutoMapper;
using fitness.Application.Features.Clients.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Clients.Queries.GetClientMealPlans;

public class GetClientMealPlansHandler
    : IRequestHandler<GetClientMealPlansQuery, List<MealPlanDto>>
{
    private readonly IClientManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetClientMealPlansHandler(
        IClientManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<MealPlanDto>> Handle(
        GetClientMealPlansQuery request,
        CancellationToken cancellationToken)
    {
        var assignments = await _repository.GetMealPlansAsync(
            request.UserId,
            cancellationToken);

        return _mapper.Map<List<MealPlanDto>>(assignments);
    }
}