using fitness.Domain.Entities;
using fitness.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Infrastructure.Persistence;

public class FitnessDbContext : IdentityDbContext<ApplicationUser>
{

    public FitnessDbContext(DbContextOptions<FitnessDbContext> options
        ) : base(options) { }

    public DbSet<Coach> Coaches => Set<Coach>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<WorkoutDay> WorkoutDays => Set<WorkoutDay>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();
    public DbSet<Meal> Meals => Set<Meal>();
    public DbSet<MealFood> MealFoods => Set<MealFood>();
    public DbSet<ProgressRecord> ProgressRecords => Set<ProgressRecord>();
    public DbSet<WorkoutPlanAssignment> WorkoutPlanAssignments => Set<WorkoutPlanAssignment>();
    public DbSet<MealPlanAssignment> MealPlanAssignments => Set<MealPlanAssignment>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Client - Coach (Optional)
        modelBuilder.Entity<Client>()
            .HasOne(c => c.Coach)
            .WithMany(c => c.Clients)
            .HasForeignKey(c => c.CoachId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // Coach - Exercise
        modelBuilder.Entity<Exercise>()
            .HasOne(e => e.Coach)
            .WithMany()
            .HasForeignKey(e => e.CoachId)
            .OnDelete(DeleteBehavior.Restrict);

        // Coach - WorkoutPlan
        modelBuilder.Entity<WorkoutPlan>()
            .HasOne(w => w.Coach)
            .WithMany()
            .HasForeignKey(w => w.CoachId)
            .OnDelete(DeleteBehavior.Restrict);

        // Coach - MealPlan
        modelBuilder.Entity<MealPlan>()
            .HasOne(m => m.Coach)
            .WithMany()
            .HasForeignKey(m => m.CoachId)
            .OnDelete(DeleteBehavior.Restrict);


        // ApplicationUser - Client (One-to-One)
        modelBuilder.Entity<Client>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        // ApplicationUser - Coach (One-to-One)
        modelBuilder.Entity<Coach>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Coach>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        // WorkoutPlan - WorkoutDay
        modelBuilder.Entity<WorkoutDay>()
            .HasOne(wd => wd.WorkoutPlan)
            .WithMany(wp => wp.WorkoutDays)
            .HasForeignKey(wd => wd.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        // WorkoutDay - WorkoutExercise
        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(we => we.WorkoutDay)
            .WithMany(wd => wd.WorkoutExercises)
            .HasForeignKey(we => we.WorkoutDayId)
            .OnDelete(DeleteBehavior.Cascade);

        // Exercise - WorkoutExercise
        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(we => we.Exercise)
            .WithMany()
            .HasForeignKey(we => we.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        // MealPlan - Meal
        modelBuilder.Entity<Meal>()
            .HasOne(m => m.MealPlan)
            .WithMany(mp => mp.Meals)
            .HasForeignKey(m => m.MealPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Meal - MealFood
        modelBuilder.Entity<MealFood>()
            .HasOne(mf => mf.Meal)
            .WithMany(m => m.Foods)
            .HasForeignKey(mf => mf.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        // Client - ProgressRecord
        modelBuilder.Entity<ProgressRecord>()
            .HasOne(pr => pr.Client)
            .WithMany(c => c.ProgressRecords)
            .HasForeignKey(pr => pr.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Client - WorkoutPlanAssignment
        modelBuilder.Entity<WorkoutPlanAssignment>()
            .HasOne(a => a.Client)
            .WithMany(c => c.WorkoutPlanAssignments)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        // WorkoutPlan - WorkoutPlanAssignment
        modelBuilder.Entity<WorkoutPlanAssignment>()
            .HasOne(a => a.WorkoutPlan)
            .WithMany(wp => wp.Assignments)
            .HasForeignKey(a => a.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Client - MealPlanAssignment
        modelBuilder.Entity<MealPlanAssignment>()
            .HasOne(a => a.Client)
            .WithMany(c => c.MealPlanAssignments)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        // MealPlan - MealPlanAssignment
        modelBuilder.Entity<MealPlanAssignment>()
            .HasOne(a => a.MealPlan)
            .WithMany(mp => mp.Assignments)
            .HasForeignKey(a => a.MealPlanId)
            .OnDelete(DeleteBehavior.Restrict);


        // Client
        modelBuilder.Entity<Client>()
            .Property(c => c.Height)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Client>()
            .Property(c => c.CurrentWeight)
            .HasPrecision(18, 2);

        // Meal
        modelBuilder.Entity<Meal>()
            .Property(m => m.Protein).HasPrecision(18, 2);

        modelBuilder.Entity<Meal>()
            .Property(m => m.Carbs).HasPrecision(18, 2);

        modelBuilder.Entity<Meal>()
            .Property(m => m.Fat).HasPrecision(18, 2);

        // MealFood
        modelBuilder.Entity<MealFood>()
            .Property(m => m.Quantity).HasPrecision(18, 2);

        modelBuilder.Entity<MealFood>()
            .Property(m => m.Protein).HasPrecision(18, 2);

        modelBuilder.Entity<MealFood>()
            .Property(m => m.Carbs).HasPrecision(18, 2);

        modelBuilder.Entity<MealFood>()
            .Property(m => m.Fat).HasPrecision(18, 2);

        // MealPlan
        modelBuilder.Entity<MealPlan>()
            .Property(m => m.ProteinTarget).HasPrecision(18, 2);

        modelBuilder.Entity<MealPlan>()
            .Property(m => m.CarbsTarget).HasPrecision(18, 2);

        modelBuilder.Entity<MealPlan>()
            .Property(m => m.FatTarget).HasPrecision(18, 2);

        // ProgressRecord
        modelBuilder.Entity<ProgressRecord>()
            .Property(p => p.Weight).HasPrecision(18, 2);

        modelBuilder.Entity<ProgressRecord>()
            .Property(p => p.BodyFatPercentage).HasPrecision(18, 2);

        modelBuilder.Entity<ProgressRecord>()
            .Property(p => p.MuscleMass).HasPrecision(18, 2);


        modelBuilder.Entity<RefreshToken>()
    .HasOne<ApplicationUser>()
    .WithMany()
    .HasForeignKey(rt => rt.UserId)
    .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<RefreshToken>()
    .HasIndex(rt => rt.UserId)
    .IsUnique();
    }


}
