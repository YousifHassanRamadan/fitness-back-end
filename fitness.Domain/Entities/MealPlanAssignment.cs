namespace fitness.Domain.Entities;

public class MealPlanAssignment
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int MealPlanId { get; set; }
    public MealPlan MealPlan { get; set; } = null!;

    public DateTime AssignedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }


}