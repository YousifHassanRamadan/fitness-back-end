using AutoMapper;
using fitness.Application.Features.Coaches.DTOs;
using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Queries.GetCoachMealPlanById;

public class GetCoachMealPlanByIdQueryHandler
    : IRequestHandler<GetCoachMealPlanByIdQuery, CoachMealPlanDetailsDto?>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public GetCoachMealPlanByIdQueryHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CoachMealPlanDetailsDto?> Handle(
        GetCoachMealPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var mealPlan = await _repository.GetMealPlanDetailsByCoachAsync(
            request.MealPlanId,
            request.UserId,
            cancellationToken);

        return mealPlan == null
            ? null
            : _mapper.Map<CoachMealPlanDetailsDto>(mealPlan);
    }
}