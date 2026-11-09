using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class WorkoutExercise
{

    public int Id { get; set; }

    public int WorkoutDayId { get; set; }

    public WorkoutDay WorkoutDay { get; set; } = null!;

    public int ExerciseId { get; set; }

    public Exercise Exercise { get; set; } = null!;

    public int Sets { get; set; }

    public int Reps { get; set; }

    public int RestSeconds { get; set; }

    public int Order { get; set; }
}
