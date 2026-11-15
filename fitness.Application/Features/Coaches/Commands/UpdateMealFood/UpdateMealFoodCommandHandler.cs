using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateMealFood;

public class UpdateMealFoodCommandHandler
    : IRequestHandler<UpdateMealFoodCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public UpdateMealFoodCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateMealFoodCommand request,
        CancellationToken cancellationToken)
    {
        var mealFood = await _repository.GetMealFoodByCoachAsync(
            request.MealFoodId,
            request.UserId,
            cancellationToken);

        if (mealFood == null)
            return false;

        mealFood.FoodName = request.FoodName;
        mealFood.Quantity = request.Quantity;
        mealFood.Unit = request.Unit;
        mealFood.Calories = request.Calories;
        mealFood.Protein = request.Protein;
        mealFood.Carbs = request.Carbs;
        mealFood.Fat = request.Fat;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}