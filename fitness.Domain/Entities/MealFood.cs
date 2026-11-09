using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class MealFood
{
    public int Id { get; set; }

    public int MealId { get; set; }
    public Meal Meal { get; set; } = null!;

    public string FoodName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;

    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }
}
