using AutoMapper;
using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.CreateMeal;

public class CreateMealCommandHandler
    : IRequestHandler<CreateMealCommand, int>
{
    private readonly ICoachManagementRepository _repository;
    private readonly IMapper _mapper;

    public CreateMealCommandHandler(
        ICoachManagementRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(
        CreateMealCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Meal name is required.");

        if (request.Calories < 0 ||
            request.Protein < 0 ||
            request.Carbs < 0 ||
            request.Fat < 0)
            throw new ArgumentException("Invalid nutritional values.");

        var mealPlan = await _repository.GetMealPlanDetailsByCoachAsync(
            request.MealPlanId,
            request.UserId,
            cancellationToken);

        if (mealPlan == null)
            throw new InvalidOperationException("Meal plan not found.");

        var meal = _mapper.Map<Meal>(request);
        meal.MealPlanId = mealPlan.Id;

        await _repository.AddMealAsync(meal, cancellationToken);

        return meal.Id;
    }
}