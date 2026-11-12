
using fitness.Domain.Entities;

namespace fitness.Application.Interfaces;

public interface ICoachManagementRepository
{
    Task<Coach?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<List<Client>> GetClientsAsync(
        string coachUserId,
        CancellationToken cancellationToken);

    Task<Client?> GetClientDetailsAsync(
        string coachUserId,
        int clientId,
        CancellationToken cancellationToken);

    Task<Client?> GetClientByIdAsync(
    int clientId,
    CancellationToken cancellationToken);

    Task AddWorkoutPlanAsync(
    WorkoutPlan workoutPlan,
    CancellationToken cancellationToken);


    Task<WorkoutPlan?> GetWorkoutPlanByCoachAsync(
    int workoutPlanId,
    string coachUserId,
    CancellationToken cancellationToken);

    Task AddWorkoutDayAsync(
        WorkoutDay workoutDay,
        CancellationToken cancellationToken);


    Task<WorkoutDay?> GetWorkoutDayByCoachAsync(
    int workoutDayId,
    string coachUserId,
    CancellationToken cancellationToken);

    Task<Exercise?> GetExerciseByCoachAsync(
        int exerciseId,
        string coachUserId,
        CancellationToken cancellationToken);

    Task AddWorkoutExerciseAsync(
        WorkoutExercise workoutExercise,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}
