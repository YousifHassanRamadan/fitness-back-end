using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class WorkoutDay
{
    public int Id { get; set; }

    public int WorkoutPlanId { get; set; }

    public WorkoutPlan WorkoutPlan { get; set; } = null!;

    public int DayNumber { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
        = new List<WorkoutExercise>();
}
