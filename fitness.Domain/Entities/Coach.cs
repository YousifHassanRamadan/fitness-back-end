
namespace fitness.Domain.Entities;

public class Coach
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Bio { get; set; }

    public ICollection<Client> Clients { get; set; }
        = new List<Client>();
}
