using fitness.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class Meal
{
    public int Id { get; set; }

    public int MealPlanId { get; set; }
    public MealPlan MealPlan { get; set; } = null!;

    public string Name { get; set; } = null!;
    public MealType MealType { get; set; }

    public string? Description { get; set; }

    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }

    public ICollection<MealFood> Foods { get; set; } = new List<MealFood>();
}
