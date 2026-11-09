using fitness.Domain.Entities;
using fitness.Domain.Enums;
using System.Reflection;

namespace fitness.Domain.Entities;

public class Client
{
    public int Id { get; set; }

    // Identity relationship
    public string UserId { get; set; } = null!;

    // Personal information
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public Gender Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; } = null!;

    // Coach relationship
    public int? CoachId { get; set; }

    public Coach? Coach { get; set; }

    // Physical information
    public decimal? Height { get; set; }

    public decimal? CurrentWeight { get; set; }

    // Relationships
    public ICollection<ProgressRecord> ProgressRecords { get; set; }
        = new List<ProgressRecord>();

    public ICollection<WorkoutPlanAssignment> WorkoutPlanAssignments { get; set; }
        = new List<WorkoutPlanAssignment>();

    public ICollection<MealPlanAssignment> MealPlanAssignments { get; set; }
        = new List<MealPlanAssignment>();
}