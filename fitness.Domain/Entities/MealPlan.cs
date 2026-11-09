using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class MealPlan
{

    public int Id { get; set; }

    public int CoachId { get; set; }

    public Coach Coach { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationInWeeks { get; set; }

    public int DurationInDays { get; set; }

    public int DailyCalorieTarget { get; set; }

    public decimal ProteinTarget { get; set; }

    public decimal CarbsTarget { get; set; }

    public decimal FatTarget { get; set; }

    public ICollection<Meal> Meals { get; set; }
        = new List<Meal>();


    public ICollection<MealPlanAssignment> Assignments { get; set; }
    = new List<MealPlanAssignment>();
}
