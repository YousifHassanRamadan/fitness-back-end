namespace fitness.Application.Features.Coaches.DTOs;

public class CoachMealPlanDetailsDto
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

    public List<CoachMealDto> Meals { get; set; } = [];
}

public class CoachMealDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public fitness.Domain.Enums.MealType MealType { get; set; }
    public string? Description { get; set; }
    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }

    public List<CoachMealFoodDto> Foods { get; set; } = [];
}

public class CoachMealFoodDto
{
    public int Id { get; set; }
    public string FoodName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;
    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }
}