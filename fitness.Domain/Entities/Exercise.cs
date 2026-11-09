using fitness.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class Exercise
{

    public int Id { get; set; }

    public int CoachId { get; set; }

    public Coach Coach { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public MuscleGroup MuscleGroup { get; set; }

    public string? Equipment { get; set; }

    public string? VideoUrl { get; set; }
}
