using AutoMapper;
using fitness.Application.Interfaces;
using fitness.Application.Features.Coaches.DTOs;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachMealPlans;

public class GetCoachMealPlansQueryHandler
    : IRequestHandler<GetCoachMealPlansQuery, List<CoachMealPlanDto>>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetCoachMealPlansQueryHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CoachMealPlanDto>> Handle(
        GetCoachMealPlansQuery request,
        CancellationToken cancellationToken)
    {
        var mealPlans = await _repository.GetMealPlansByCoachAsync(
            request.UserId,
            cancellationToken);

        return _mapper.Map<List<CoachMealPlanDto>>(mealPlans);
    }
}