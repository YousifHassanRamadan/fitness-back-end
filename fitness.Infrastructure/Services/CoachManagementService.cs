
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

    public async Task<List<WorkoutPlan>> GetWorkoutPlansByCoachAsync(
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.WorkoutPlans
            .Where(p => p.Coach.UserId == coachUserId)
            .ToListAsync(cancellationToken);
    }


    public async Task<WorkoutPlan?> GetWorkoutPlanDetailsByCoachAsync(
    int workoutPlanId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.WorkoutPlans
            .Where(p => p.Id == workoutPlanId &&
                        p.Coach.UserId == coachUserId)
            .Include(p => p.WorkoutDays)
                .ThenInclude(d => d.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteWorkoutPlanAsync(
    WorkoutPlan workoutPlan,
    CancellationToken cancellationToken)
    {
        _context.WorkoutPlans.Remove(workoutPlan);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteWorkoutDayAsync(
    WorkoutDay workoutDay,
    CancellationToken cancellationToken)
    {
        _context.WorkoutDays.Remove(workoutDay);

        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task AddExerciseAsync(
    Exercise exercise,
    CancellationToken cancellationToken)
    {
        await _context.Exercises.AddAsync(exercise, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<List<Exercise>> GetExercisesByCoachAsync(
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .Where(e => e.Coach.UserId == coachUserId)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteExerciseAsync(
    Exercise exercise,
    CancellationToken cancellationToken)
    {
        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<WorkoutExercise?> GetWorkoutExerciseByCoachAsync(
    int workoutExerciseId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.WorkoutExercises
            .FirstOrDefaultAsync(
                we => we.Id == workoutExerciseId &&
                      we.WorkoutDay.WorkoutPlan.Coach.UserId == coachUserId,
                cancellationToken);
    }

    public async Task DeleteWorkoutExerciseAsync(
    WorkoutExercise workoutExercise,
    CancellationToken cancellationToken)
    {
        _context.WorkoutExercises.Remove(workoutExercise);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddWorkoutPlanAssignmentAsync(
    WorkoutPlanAssignment assignment,
    CancellationToken cancellationToken)
    {
        _context.WorkoutPlanAssignments.Add(assignment);

        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task<WorkoutPlanAssignment?> GetWorkoutPlanAssignmentByCoachAsync(
    int assignmentId,
    int clientId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.WorkoutPlanAssignments
            .FirstOrDefaultAsync(
                a => a.Id == assignmentId &&
                     a.ClientId == clientId &&
                     a.Client.Coach!.UserId == coachUserId,
                cancellationToken);
    }

    public async Task DeleteWorkoutPlanAssignmentAsync(
        WorkoutPlanAssignment assignment,
        CancellationToken cancellationToken)
    {
        _context.WorkoutPlanAssignments.Remove(assignment);

        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task AddMealPlanAsync(
    MealPlan mealPlan,
    CancellationToken cancellationToken)
    {
        _context.MealPlans.Add(mealPlan);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<MealPlan>> GetMealPlansByCoachAsync(
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.MealPlans
            .Where(mp => mp.Coach.UserId == coachUserId)
            .OrderByDescending(mp => mp.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<MealPlan?> GetMealPlanDetailsByCoachAsync(
    int mealPlanId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.MealPlans
            .Include(mp => mp.Meals)
                .ThenInclude(m => m.Foods)
            .FirstOrDefaultAsync(
                mp => mp.Id == mealPlanId &&
                      mp.Coach.UserId == coachUserId,
                cancellationToken);
    }


    public async Task DeleteMealPlanAsync(
    MealPlan mealPlan,
    CancellationToken cancellationToken)
    {
        _context.MealPlans.Remove(mealPlan);

        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task AddMealAsync(
    Meal meal,
    CancellationToken cancellationToken)
    {
        _context.Meals.Add(meal);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Meal?> GetMealByCoachAsync(
    int mealId,
    string coachUserId,
    CancellationToken cancellationToken)
    {
        return await _context.Meals
            .Include(m => m.MealPlan)
            .FirstOrDefaultAsync(
                m => m.Id == mealId &&
                     m.MealPlan.Coach.UserId == coachUserId,
                cancellationToken);
    }

    // Delete Meal
    public async Task DeleteMealAsync(
        Meal meal,
        CancellationToken cancellationToken)
    {
        _context.Meals.Remove(meal);

        await _context.SaveChangesAsync(cancellationToken);
    }

    // Add Meal Food
    public async Task AddMealFoodAsync(
        MealFood mealFood,
        CancellationToken cancellationToken)
    {
        await _context.Set<MealFood>().AddAsync(
            mealFood,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    // Get Meal Food by Coach
    public async Task<MealFood?> GetMealFoodByCoachAsync(
        int mealFoodId,
        string coachUserId,
        CancellationToken cancellationToken)
    {
        return await _context.Set<MealFood>()
            .Include(f => f.Meal)
                .ThenInclude(m => m.MealPlan)
            .FirstOrDefaultAsync(
                f => f.Id == mealFoodId &&
                     f.Meal.MealPlan.Coach.UserId == coachUserId,
                cancellationToken);
    }

    // Delete Meal Food
    public async Task DeleteMealFoodAsync(
        MealFood mealFood,
        CancellationToken cancellationToken)
    {
        _context.Set<MealFood>().Remove(mealFood);

        await _context.SaveChangesAsync(cancellationToken);
    }

    // Add Meal Plan Assignment
    public async Task AddMealPlanAssignmentAsync(
        MealPlanAssignment assignment,
        CancellationToken cancellationToken)
    {
        await _context.Set<MealPlanAssignment>().AddAsync(
            assignment,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    // Get Meal Plan Assignment by Coach
    public async Task<MealPlanAssignment?> GetMealPlanAssignmentByCoachAsync(
        int assignmentId,
        int clientId,
        string coachUserId,
        CancellationToken cancellationToken)
    {
        return await _context.Set<MealPlanAssignment>()
            .FirstOrDefaultAsync(
                a => a.Id == assignmentId &&
                     a.ClientId == clientId &&
                     a.Client.Coach != null &&
                     a.Client.Coach.UserId == coachUserId,
                cancellationToken);
    }

    // Delete Meal Plan Assignment
    public async Task DeleteMealPlanAssignmentAsync(
        MealPlanAssignment assignment,
        CancellationToken cancellationToken)
    {
        _context.Set<MealPlanAssignment>().Remove(assignment);

        await _context.SaveChangesAsync(cancellationToken);
    }

}
