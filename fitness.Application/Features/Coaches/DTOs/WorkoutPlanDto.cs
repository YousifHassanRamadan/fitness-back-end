namespace fitness.Application.Features.Coaches.DTOs;

public class WorkoutPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationInWeeks { get; set; }
}