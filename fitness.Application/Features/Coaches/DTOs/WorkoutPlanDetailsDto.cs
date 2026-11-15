namespace fitness.Application.Features.Coaches.DTOs;

public class WorkoutPlanDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationInWeeks { get; set; }

    public List<WorkoutDayDto> WorkoutDays { get; set; } = new();
}

public class WorkoutDayDto
{
    public int Id { get; set; }
    public int DayNumber { get; set; }
    public string Name { get; set; } = null!;

    public List<WorkoutExerciseDto> Exercises { get; set; } = new();
}

public class WorkoutExerciseDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } = null!;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int RestSeconds { get; set; }
    public int Order { get; set; }
}