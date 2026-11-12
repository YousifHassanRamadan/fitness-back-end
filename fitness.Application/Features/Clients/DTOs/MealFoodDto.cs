namespace fitness.Application.Features.Clients.DTOs;

public class MealFoodDto
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