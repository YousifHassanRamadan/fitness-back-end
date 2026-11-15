using fitness.Domain.Enums;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.UpdateMeal;

public record UpdateMealCommand : IRequest<bool>
{
    public string UserId { get; init; } = null!;
    public int MealId { get; init; }

    public string Name { get; init; } = null!;
    public MealType MealType { get; init; }
    public string? Description { get; init; }

    public int Calories { get; init; }
    public decimal Protein { get; init; }
    public decimal Carbs { get; init; }
    public decimal Fat { get; init; }
}