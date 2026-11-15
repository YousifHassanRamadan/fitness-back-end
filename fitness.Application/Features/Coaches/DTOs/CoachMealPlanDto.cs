namespace fitness.Application.Features.Coaches.DTOs;

public class CoachMealPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationInWeeks { get; set; }
    public int DurationInDays { get; set; }
    public int DailyCalorieTarget { get; set; }
    public decimal ProteinTarget { get; set; }
    public decimal CarbsTarget { get; set; }
    public decimal FatTarget { get; set; }
}