using fitness.Domain.Entities;

namespace fitness.Application.Interfaces;

public interface IClientManagementRepository
{
    Task<Client?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<List<WorkoutPlanAssignment>> GetWorkoutPlansAsync(
    string userId,
    CancellationToken cancellationToken);

    Task<List<ProgressRecord>> GetProgressRecordsAsync(
    string userId,
    CancellationToken cancellationToken);

    Task<List<MealPlanAssignment>> GetMealPlansAsync(
    string userId,
    CancellationToken cancellationToken);



    Task SaveChangesAsync(CancellationToken cancellationToken);
}