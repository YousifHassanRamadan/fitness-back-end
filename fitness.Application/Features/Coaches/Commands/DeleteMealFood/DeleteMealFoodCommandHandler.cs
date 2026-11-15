using fitness.Application.Interfaces;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.DeleteMealFood;

public class DeleteMealFoodCommandHandler
    : IRequestHandler<DeleteMealFoodCommand, bool>
{
    private readonly ICoachManagementRepository _repository;

    public DeleteMealFoodCommandHandler(
        ICoachManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteMealFoodCommand request,
        CancellationToken cancellationToken)
    {
        var mealFood = await _repository.GetMealFoodByCoachAsync(
            request.MealFoodId,
            request.UserId,
            cancellationToken);

        if (mealFood == null)
            return false;

        await _repository.DeleteMealFoodAsync(
            mealFood,
            cancellationToken);

        return true;
    }
}