namespace fitness.Application.Features.Clients.DTOs;

public class MealDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string MealType { get; set; } = null!;
    public string? Description { get; set; }

    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }

    public List<MealFoodDto> Foods { get; set; } = new();
}