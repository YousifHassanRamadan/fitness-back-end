namespace fitness.Application.Features.Clients.DTOs;

public class MealPlanDto
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

    public DateTime AssignedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public List<MealDto> Meals { get; set; } = new();
}