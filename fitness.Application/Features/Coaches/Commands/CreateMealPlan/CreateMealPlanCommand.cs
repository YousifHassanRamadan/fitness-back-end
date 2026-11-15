using MediatR;

namespace fitness.Application.Features.Coaches.Commands.CreateMealPlan;

public record CreateMealPlanCommand : IRequest<int>
{
    public string UserId { get; init; } = null!;

    public string Name { get; init; } = null!;
    public string? Description { get; init; }

    public int DurationInWeeks { get; init; }
    public int DurationInDays { get; init; }

    public int DailyCalorieTarget { get; init; }

    public decimal ProteinTarget { get; init; }
    public decimal CarbsTarget { get; init; }
    public decimal FatTarget { get; init; }
}