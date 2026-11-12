namespace fitness.Application.Features.Clients.DTOs;

public class WorkoutPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationInWeeks { get; set; }

    public DateTime AssignedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public List<WorkoutDayDto> WorkoutDays { get; set; } = new();
}