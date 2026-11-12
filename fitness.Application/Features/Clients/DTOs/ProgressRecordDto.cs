namespace fitness.Application.Features.Clients.DTOs;

public class ProgressRecordDto
{
    public int Id { get; set; }
    public decimal Weight { get; set; }
    public decimal? BodyFatPercentage { get; set; }
    public decimal? MuscleMass { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Notes { get; set; }
}