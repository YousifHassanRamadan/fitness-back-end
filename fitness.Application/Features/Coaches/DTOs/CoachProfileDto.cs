
namespace fitness.Application.Features.Coaches.DTOs;

public class CoachProfileDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Bio { get; set; }
    public int ClientsCount { get; set; }
}
