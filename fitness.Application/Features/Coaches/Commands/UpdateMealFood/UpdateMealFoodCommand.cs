using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateMealFood;

public record UpdateMealFoodCommand(
    string FoodName,
    decimal Quantity,
    string Unit,
    int Calories,
    decimal Protein,
    decimal Carbs,
    decimal Fat) : IRequest<bool>
{
    public string UserId { get; init; } = string.Empty;
    public int MealFoodId { get; init; }
}