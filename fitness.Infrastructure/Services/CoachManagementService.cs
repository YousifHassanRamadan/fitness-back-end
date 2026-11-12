
using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using fitness.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace fitness.Infrastructure.Services;

public class CoachManagementService
    : ICoachManagementRepository
{
    private readonly FitnessDbContext _context;

    public CoachManagementService(FitnessDbContext context)
    {
        _context = context;
    }

    public async Task<Coach?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await _context.Coaches
            .Include(c => c.Clients)
            .FirstOrDefaultAsync(
                c => c.UserId == userId,
                cancellationToken);
    }

    public async Task<List<Client>> GetClientsAsync(
        string coachUserId,
        CancellationToken cancellationToken)
    {
        return await _context.Clients
            .Where(c => c.Coach != null &&
                        c.Coach.UserId == coachUserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetClientDetailsAsync(
        string coachUserId,
        int clientId,
        CancellationToken cancellationToken)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(
                c => c.Id == clientId &&
                     c.Coach != null &&
                     c.Coach.UserId == coachUserId,
                cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<Client?> GetClientByIdAsync(
    int clientId,
    CancellationToken cancellationToken)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(
                c => c.Id == clientId,
                cancellationToken);
    }





    public async Task AddWorkoutPlanAsync(
    WorkoutPlan workoutPlan,
    CancellationToken cancellationToken)
    {
        await _context.WorkoutPlans.AddAsync(workoutPlan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }



    public async Task<WorkoutPlan?> GetWorkoutPlanByCoachAsync(
    int workoutPlanId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.WorkoutPlans
            .FirstOrDefaultAsync(
                p => p.Id == workoutPlanId &&
                     p.Coach.UserId == coachUserId,
                cancellationToken);
    }

    public async Task AddWorkoutDayAsync(
        WorkoutDay workoutDay,
        CancellationToken cancellationToken)
    {
        await _context.WorkoutDays.AddAsync(workoutDay, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<WorkoutDay?> GetWorkoutDayByCoachAsync(
    int workoutDayId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.WorkoutDays
            .FirstOrDefaultAsync(
                d => d.Id == workoutDayId &&
                     d.WorkoutPlan.Coach.UserId == coachUserId,
                cancellationToken);
    }


    public async Task<Exercise?> GetExerciseByCoachAsync(
    int exerciseId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .FirstOrDefaultAsync(
                e => e.Id == exerciseId &&
                     e.Coach.UserId == coachUserId,
                cancellationToken);
    }


    public async Task AddWorkoutExerciseAsync(
    WorkoutExercise workoutExercise,
    CancellationToken cancellationToken)
    {
        await _context.WorkoutExercises.AddAsync(
            workoutExercise,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
