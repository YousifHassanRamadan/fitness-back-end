using MediatR;

namespace fitness.Application.Features.Coaches.Commands.AddMealFood;

public record AddMealFoodCommand(
    string FoodName,
    decimal Quantity,
    string Unit,
    int Calories,
    decimal Protein,
    decimal Carbs,
    decimal Fat) : IRequest<int>
{
    public string UserId { get; init; } = string.Empty;
    public int MealId { get; init; }
}