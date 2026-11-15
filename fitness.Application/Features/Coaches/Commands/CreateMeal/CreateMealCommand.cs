using fitness.Domain.Enums;
using MediatR;

namespace fitness.Application.Features.Coaches.Commands.CreateMeal;

public record CreateMealCommand : IRequest<int>
{
    public string UserId { get; init; } = null!;
    public int MealPlanId { get; init; }

    public string Name { get; init; } = null!;
    public MealType MealType { get; init; }
    public string? Description { get; init; }

    public int Calories { get; init; }
    public decimal Protein { get; init; }
    public decimal Carbs { get; init; }
    public decimal Fat { get; init; }
}