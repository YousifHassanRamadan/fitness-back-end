using fitness.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Features.Coaches.DTOs;

public class ExerciseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public string? Equipment { get; set; }
    public string? VideoUrl { get; set; }
}
