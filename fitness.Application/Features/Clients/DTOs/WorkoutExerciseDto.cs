namespace fitness.Application.Features.Clients.DTOs;

public class WorkoutExerciseDto
{
    public int Id { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int RestSeconds { get; set; }
    public int Order { get; set; }

    public string ExerciseName { get; set; } = null!;
    public string? Description { get; set; }
    public string MuscleGroup { get; set; } = null!;
    public string? Equipment { get; set; }
    public string? VideoUrl { get; set; }
}