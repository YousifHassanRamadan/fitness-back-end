using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AddMealFood;

public class AddMealFoodCommandHandler
    : IRequestHandler<AddMealFoodCommand, int>
{
    private readonly ICoachManagementRepository _repository;

    public AddMealFoodCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        AddMealFoodCommand request,
        CancellationToken cancellationToken)
    {
        var meal = await _repository.GetMealByCoachAsync(
            request.MealId,
            request.UserId,
            cancellationToken);

        if (meal == null)
            return 0;

        var mealFood = new MealFood
        {
            MealId = meal.Id,
            FoodName = request.FoodName,
            Quantity = request.Quantity,
            Unit = request.Unit,
            Calories = request.Calories,
            Protein = request.Protein,
            Carbs = request.Carbs,
            Fat = request.Fat
        };

        await _repository.AddMealFoodAsync(
            mealFood,
            cancellationToken);

        return mealFood.Id;
    }
}