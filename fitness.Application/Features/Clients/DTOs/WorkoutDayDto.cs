namespace fitness.Application.Features.Clients.DTOs;

public class WorkoutDayDto
{
    public int Id { get; set; }
    public int DayNumber { get; set; }
    public string Name { get; set; } = null!;

    public List<WorkoutExerciseDto> Exercises { get; set; } = new();
}