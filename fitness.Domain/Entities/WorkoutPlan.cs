using fitness.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class WorkoutPlan
{
    public int Id { get; set; }

    public int CoachId { get; set; }

    public Coach Coach { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationInWeeks { get; set; }

    public ICollection<WorkoutDay> WorkoutDays { get; set; }
        = new List<WorkoutDay>();

    public ICollection<WorkoutPlanAssignment> Assignments { get; set; }
    = new List<WorkoutPlanAssignment>();

}
