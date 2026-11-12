using fitness.Application.Interfaces;
using fitness.Domain.Entities;
using fitness.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace fitness.Infrastructure.Repositories;

public class ClientManagementService : IClientManagementRepository
{
    private readonly FitnessDbContext _context;

    public ClientManagementService(FitnessDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await _context.Clients
            .Include(c => c.Coach)
            .FirstOrDefaultAsync(
                c => c.UserId == userId,
                cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<WorkoutPlanAssignment>> GetWorkoutPlansAsync(
    string userId,
    CancellationToken cancellationToken)
    {
        return await _context.WorkoutPlanAssignments
            .Where(a => a.Client.UserId == userId)
            .Include(a => a.WorkoutPlan)
                .ThenInclude(p => p.WorkoutDays)
                    .ThenInclude(d => d.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<MealPlanAssignment>> GetMealPlansAsync(
    string userId,
    CancellationToken cancellationToken)
    {
        return await _context.MealPlanAssignments
            .Where(a => a.Client.UserId == userId)
            .Include(a => a.MealPlan)
                .ThenInclude(p => p.Meals)
                    .ThenInclude(m => m.Foods)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProgressRecord>> GetProgressRecordsAsync(
    string userId,
    CancellationToken cancellationToken)
    {
        return await _context.ProgressRecords
            .Where(p => p.Client.UserId == userId)
            .OrderByDescending(p => p.RecordedAt)
            .ToListAsync(cancellationToken);
    }
}