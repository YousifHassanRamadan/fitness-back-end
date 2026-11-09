using fitness.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Domain.Entities
{
    public class WorkoutPlanAssignment
    {
        public int Id { get; set; }

        // Client
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        // Workout Plan
        public int WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; } = null!;

        // Assignment Information
        public DateTime AssignedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
