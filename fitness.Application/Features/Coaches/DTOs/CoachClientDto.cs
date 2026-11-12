
namespace fitness.Application.Features.Coaches.DTOs;

public class CoachClientDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string Address { get; set; } = null!;
    public decimal? Height { get; set; }
    public decimal? CurrentWeight { get; set; }
}
