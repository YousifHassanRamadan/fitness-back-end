using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities;

public class ProgressRecord
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public decimal Weight { get; set; }
    public decimal? BodyFatPercentage { get; set; }

    public decimal? MuscleMass { get; set; }

    public DateTime RecordedAt { get; set; }

    public string? Notes { get; set; }
}
