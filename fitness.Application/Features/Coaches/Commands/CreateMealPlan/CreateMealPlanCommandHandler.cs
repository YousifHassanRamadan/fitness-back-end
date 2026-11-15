using AutoMapper;
using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.CreateMealPlan;

public class CreateMealPlanCommandHandler
    : IRequestHandler<CreateMealPlanCommand, int>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public CreateMealPlanCommandHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(
        CreateMealPlanCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Meal plan name is required.");

        if (request.DurationInWeeks <= 0 ||
            request.DurationInDays < 0 ||
            request.DailyCalorieTarget <= 0 ||
            request.ProteinTarget < 0 ||
            request.CarbsTarget < 0 ||
            request.FatTarget < 0)
        {
            throw new ArgumentException("Invalid meal plan values.");
        }

        var coach = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (coach == null)
            throw new InvalidOperationException("Coach not found.");

        var mealPlan = _mapper.Map<MealPlan>(request);

        mealPlan.CoachId = coach.Id;

        await _repository.AddMealPlanAsync(
            mealPlan,
            cancellationToken);

        return mealPlan.Id;
    }
}