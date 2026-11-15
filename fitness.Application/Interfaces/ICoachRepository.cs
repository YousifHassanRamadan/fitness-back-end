
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

    Task<List<WorkoutPlan>> GetWorkoutPlansByCoachAsync(
    string coachUserId,
    CancellationToken cancellationToken);

    Task<WorkoutPlan?> GetWorkoutPlanDetailsByCoachAsync(
    int workoutPlanId,
    string coachUserId,
    CancellationToken cancellationToken);

    Task DeleteWorkoutPlanAsync(
    WorkoutPlan workoutPlan,
    CancellationToken cancellationToken);

    Task DeleteWorkoutDayAsync(
    WorkoutDay workoutDay,
    CancellationToken cancellationToken);

    Task AddExerciseAsync(
    Exercise exercise,
    CancellationToken cancellationToken);

    Task<List<Exercise>> GetExercisesByCoachAsync(
    string coachUserId,
    CancellationToken cancellationToken);


    Task DeleteExerciseAsync(
    Exercise exercise,
    CancellationToken cancellationToken);


    Task<WorkoutExercise?> GetWorkoutExerciseByCoachAsync(
    int workoutExerciseId,
    string coachUserId,
    CancellationToken cancellationToken);


    Task DeleteWorkoutExerciseAsync(
    WorkoutExercise workoutExercise,
    CancellationToken cancellationToken);

    Task AddWorkoutPlanAssignmentAsync(
    WorkoutPlanAssignment assignment,
    CancellationToken cancellationToken);


    Task<WorkoutPlanAssignment?> GetWorkoutPlanAssignmentByCoachAsync(
    int assignmentId,
    int clientId,
    string coachUserId,
    CancellationToken cancellationToken);

    Task DeleteWorkoutPlanAssignmentAsync(
        WorkoutPlanAssignment assignment,
        CancellationToken cancellationToken);

    Task AddMealPlanAsync(
    MealPlan mealPlan,
    CancellationToken cancellationToken);

    Task<List<MealPlan>> GetMealPlansByCoachAsync(
    string coachUserId,
    CancellationToken cancellationToken);

    Task<MealPlan?> GetMealPlanDetailsByCoachAsync(
    int mealPlanId,
    string coachUserId,
    CancellationToken cancellationToken);

    Task DeleteMealPlanAsync(
    MealPlan mealPlan,
    CancellationToken cancellationToken);

    Task AddMealAsync(
    Meal meal,
    CancellationToken cancellationToken);


    Task<Meal?> GetMealByCoachAsync(
    int mealId,
    string coachUserId,
    CancellationToken cancellationToken);


    // Meals
    Task DeleteMealAsync(
        Meal meal,
        CancellationToken cancellationToken);

    // Meal Foods
    Task AddMealFoodAsync(
        MealFood mealFood,
        CancellationToken cancellationToken);

    Task<MealFood?> GetMealFoodByCoachAsync(
        int mealFoodId,
        string coachUserId,
        CancellationToken cancellationToken);

    Task DeleteMealFoodAsync(
        MealFood mealFood,
        CancellationToken cancellationToken);

    // Meal Plan Assignments
    Task AddMealPlanAssignmentAsync(
        MealPlanAssignment assignment,
        CancellationToken cancellationToken);

    Task<MealPlanAssignment?> GetMealPlanAssignmentByCoachAsync(
        int assignmentId,
        int clientId,
        string coachUserId,
        CancellationToken cancellationToken);

    Task DeleteMealPlanAssignmentAsync(
        MealPlanAssignment assignment,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}
