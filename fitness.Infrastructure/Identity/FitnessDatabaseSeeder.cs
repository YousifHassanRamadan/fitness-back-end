using fitness.Domain.Entities;
using fitness.Domain.Enums;
using fitness.Infrastructure.Identity;
using fitness.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace fitness.Infrastructure.Identity;

public static class FitnessDatabaseSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var context =
            services.GetRequiredService<FitnessDbContext>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        // Coach credentials: supports your existing User Secrets.
        var coachEmail =
            configuration["SeedCoach:Email"]
            ?? configuration["SeedAdmin:Email"];

        var coachPassword =
            configuration["SeedCoach:Password"]
            ?? configuration["SeedAdmin:Password"];

        var clientEmail = configuration["SeedClient:Email"];
        var clientPassword = configuration["SeedClient:Password"];

        if (string.IsNullOrWhiteSpace(coachEmail) ||
            string.IsNullOrWhiteSpace(coachPassword))
        {
            throw new InvalidOperationException(
                "Coach seed credentials are missing.");
        }

        if (string.IsNullOrWhiteSpace(clientEmail) ||
            string.IsNullOrWhiteSpace(clientPassword))
        {
            throw new InvalidOperationException(
                "Client seed credentials are missing.");
        }

        // 1. Ensure roles exist.
        await EnsureRoleAsync(roleManager, "Admin");
        await EnsureRoleAsync(roleManager, "Coach");
        await EnsureRoleAsync(roleManager, "Client");

        // 2. Create Identity accounts and assign roles.
        var coachUser = await EnsureUserAsync(
            userManager,
            coachEmail,
            coachPassword,
            "Coach");

        var clientUser = await EnsureUserAsync(
            userManager,
            clientEmail,
            clientPassword,
            "Client");

        // 3. Create Coach profile.
        var coach = await context.Coaches
            .FirstOrDefaultAsync(c => c.UserId == coachUser.Id);

        if (coach == null)
        {
            coach = new Coach
            {
                UserId = coachUser.Id,
                FirstName =
                    configuration["SeedAdmin:FirstName"] ?? "Demo",
                LastName =
                    configuration["SeedAdmin:LastName"] ?? "Coach",
                Bio = "Seeded coach profile"
            };

            await context.Coaches.AddAsync(coach);
            await context.SaveChangesAsync();
        }

        // 4. Create Client profile.
        var client = await context.Clients
            .FirstOrDefaultAsync(c => c.UserId == clientUser.Id);

        if (client == null)
        {
            client = new Client
            {
                UserId = clientUser.Id,
                FirstName =
                    configuration["SeedClient:FirstName"] ?? "Demo",
                LastName =
                    configuration["SeedClient:LastName"] ?? "Client",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1998, 5, 14),
                Address = "Demo Address",
                CoachId = coach.Id,
                Height = 175m,
                CurrentWeight = 80.5m
            };

            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
        }
        else if (client.CoachId != coach.Id)
        {
            // This is the dedicated seeded test client.
            client.CoachId = coach.Id;
            await context.SaveChangesAsync();
        }

        // 5. Seed Exercises.
        var exerciseDefinitions = new[]
        {
            new Exercise
            {
                CoachId = coach.Id,
                Name = "Bench Press",
                Description = "Barbell chest press",
                MuscleGroup = MuscleGroup.Chest,
                Equipment = "Barbell"
            },
            new Exercise
            {
                CoachId = coach.Id,
                Name = "Incline Dumbbell Press",
                Description = "Incline chest press",
                MuscleGroup = MuscleGroup.Chest,
                Equipment = "Dumbbells"
            },
            new Exercise
            {
                CoachId = coach.Id,
                Name = "Lat Pulldown",
                Description = "Vertical pulling exercise",
                MuscleGroup = MuscleGroup.Back,
                Equipment = "Cable Machine"
            },
            new Exercise
            {
                CoachId = coach.Id,
                Name = "Seated Cable Row",
                Description = "Horizontal pulling exercise",
                MuscleGroup = MuscleGroup.Back,
                Equipment = "Cable Machine"
            },
            new Exercise
            {
                CoachId = coach.Id,
                Name = "Barbell Squat",
                Description = "Compound leg exercise",
                MuscleGroup = MuscleGroup.Legs,
                Equipment = "Barbell"
            },
            new Exercise
            {
                CoachId = coach.Id,
                Name = "Plank",
                Description = "Core stability exercise",
                MuscleGroup = MuscleGroup.Abs,
                Equipment = "Bodyweight"
            }
        };

        foreach (var definition in exerciseDefinitions)
        {
            var exists = await context.Exercises.AnyAsync(
                e => e.CoachId == coach.Id &&
                     e.Name == definition.Name);

            if (!exists)
            {
                await context.Exercises.AddAsync(definition);
            }
        }

        await context.SaveChangesAsync();

        var exercises = await context.Exercises
            .Where(e => e.CoachId == coach.Id)
            .ToDictionaryAsync(e => e.Name);

        // 6. Seed Workout Plan and its children.
        var workoutPlan = await context.WorkoutPlans
            .FirstOrDefaultAsync(
                p => p.CoachId == coach.Id &&
                     p.Name == "Seeded Full Body Program");

        if (workoutPlan == null)
        {
            workoutPlan = new WorkoutPlan
            {
                CoachId = coach.Id,
                Name = "Seeded Full Body Program",
                Description = "Demo three-day workout program",
                DurationInWeeks = 4,

                WorkoutDays = new List<WorkoutDay>
                {
                    new WorkoutDay
                    {
                        DayNumber = 1,
                        Name = "Push Day",
                        WorkoutExercises = new List<WorkoutExercise>
                        {
                            new WorkoutExercise
                            {
                                ExerciseId = exercises["Bench Press"].Id,
                                Sets = 4,
                                Reps = 8,
                                RestSeconds = 90,
                                Order = 1
                            },
                            new WorkoutExercise
                            {
                                ExerciseId = exercises["Incline Dumbbell Press"].Id,
                                Sets = 3,
                                Reps = 10,
                                RestSeconds = 60,
                                Order = 2
                            }
                        }
                    },
                    new WorkoutDay
                    {
                        DayNumber = 2,
                        Name = "Pull Day",
                        WorkoutExercises = new List<WorkoutExercise>
                        {
                            new WorkoutExercise
                            {
                                ExerciseId = exercises["Lat Pulldown"].Id,
                                Sets = 4,
                                Reps = 10,
                                RestSeconds = 60,
                                Order = 1
                            },
                            new WorkoutExercise
                            {
                                ExerciseId = exercises["Seated Cable Row"].Id,
                                Sets = 3,
                                Reps = 12,
                                RestSeconds = 60,
                                Order = 2
                            }
                        }
                    },
                    new WorkoutDay
                    {
                        DayNumber = 3,
                        Name = "Legs and Core",
                        WorkoutExercises = new List<WorkoutExercise>
                        {
                            new WorkoutExercise
                            {
                                ExerciseId = exercises["Barbell Squat"].Id,
                                Sets = 4,
                                Reps = 10,
                                RestSeconds = 90,
                                Order = 1
                            },
                            new WorkoutExercise
                            {
                                ExerciseId = exercises["Plank"].Id,
                                Sets = 3,
                                Reps = 30,
                                RestSeconds = 45,
                                Order = 2
                            }
                        }
                    }
                }
            };

            await context.WorkoutPlans.AddAsync(workoutPlan);
            await context.SaveChangesAsync();
        }

        // 7. Seed Workout Plan Assignment.
        var workoutAssignmentExists =
            await context.WorkoutPlanAssignments.AnyAsync(
                a => a.ClientId == client.Id &&
                     a.WorkoutPlanId == workoutPlan.Id);

        if (!workoutAssignmentExists)
        {
            await context.WorkoutPlanAssignments.AddAsync(
                new WorkoutPlanAssignment
                {
                    ClientId = client.Id,
                    WorkoutPlanId = workoutPlan.Id,
                    AssignedAt = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = null
                });

            await context.SaveChangesAsync();
        }

        // 8. Seed Meal Plan and its children.
        var mealPlan = await context.MealPlans
            .FirstOrDefaultAsync(
                p => p.CoachId == coach.Id &&
                     p.Name == "Seeded Nutrition Plan");

        if (mealPlan == null)
        {
            mealPlan = new MealPlan
            {
                CoachId = coach.Id,
                Name = "Seeded Nutrition Plan",
                Description = "Demo balanced nutrition plan",
                DurationInWeeks = 4,
                DurationInDays = 28,
                DailyCalorieTarget = 2200,
                ProteinTarget = 160m,
                CarbsTarget = 220m,
                FatTarget = 70m,

                Meals = new List<Meal>
                {
                    new Meal
                    {
                        Name = "Oats and Greek Yogurt",
                        MealType = MealType.Breakfast,
                        Description = "Breakfast meal",
                        Calories = 350,
                        Protein = 25m,
                        Carbs = 50m,
                        Fat = 7m,
                        Foods = new List<MealFood>
                        {
                            new MealFood
                            {
                                FoodName = "Rolled Oats",
                                Quantity = 50m,
                                Unit = "g",
                                Calories = 190,
                                Protein = 6.5m,
                                Carbs = 32m,
                                Fat = 3.5m
                            },
                            new MealFood
                            {
                                FoodName = "Greek Yogurt",
                                Quantity = 170m,
                                Unit = "g",
                                Calories = 100,
                                Protein = 17m,
                                Carbs = 6m,
                                Fat = 0m
                            }
                        }
                    },
                    new Meal
                    {
                        Name = "Chicken and Rice",
                        MealType = MealType.Lunch,
                        Description = "Chicken breast with rice",
                        Calories = 550,
                        Protein = 50m,
                        Carbs = 65m,
                        Fat = 8m,
                        Foods = new List<MealFood>
                        {
                            new MealFood
                            {
                                FoodName = "Chicken Breast",
                                Quantity = 150m,
                                Unit = "g",
                                Calories = 248,
                                Protein = 46m,
                                Carbs = 0m,
                                Fat = 5m
                            },
                            new MealFood
                            {
                                FoodName = "Cooked Rice",
                                Quantity = 150m,
                                Unit = "g",
                                Calories = 195,
                                Protein = 4m,
                                Carbs = 42m,
                                Fat = 0.4m
                            }
                        }
                    },
                    new Meal
                    {
                        Name = "Salmon and Potatoes",
                        MealType = MealType.Dinner,
                        Description = "Salmon with baked potatoes",
                        Calories = 600,
                        Protein = 38m,
                        Carbs = 50m,
                        Fat = 25m,
                        Foods = new List<MealFood>
                        {
                            new MealFood
                            {
                                FoodName = "Salmon",
                                Quantity = 140m,
                                Unit = "g",
                                Calories = 280,
                                Protein = 30m,
                                Carbs = 0m,
                                Fat = 18m
                            },
                            new MealFood
                            {
                                FoodName = "Potatoes",
                                Quantity = 200m,
                                Unit = "g",
                                Calories = 174,
                                Protein = 4m,
                                Carbs = 40m,
                                Fat = 0.2m
                            }
                        }
                    },
                    new Meal
                    {
                        Name = "Banana and Peanut Butter",
                        MealType = MealType.Snack,
                        Description = "Simple snack",
                        Calories = 250,
                        Protein = 7m,
                        Carbs = 30m,
                        Fat = 12m,
                        Foods = new List<MealFood>
                        {
                            new MealFood
                            {
                                FoodName = "Banana",
                                Quantity = 1m,
                                Unit = "piece",
                                Calories = 105,
                                Protein = 1.3m,
                                Carbs = 27m,
                                Fat = 0.4m
                            },
                            new MealFood
                            {
                                FoodName = "Peanut Butter",
                                Quantity = 15m,
                                Unit = "g",
                                Calories = 90,
                                Protein = 4m,
                                Carbs = 3m,
                                Fat = 8m
                            }
                        }
                    }
                }
            };

            await context.MealPlans.AddAsync(mealPlan);
            await context.SaveChangesAsync();
        }

        // 9. Seed Meal Plan Assignment.
        var mealAssignmentExists =
            await context.MealPlanAssignments.AnyAsync(
                a => a.ClientId == client.Id &&
                     a.MealPlanId == mealPlan.Id);

        if (!mealAssignmentExists)
        {
            await context.MealPlanAssignments.AddAsync(
                new MealPlanAssignment
                {
                    ClientId = client.Id,
                    MealPlanId = mealPlan.Id,
                    AssignedAt = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = null
                });

            await context.SaveChangesAsync();
        }

        // 10. Seed Progress Records.
        var progressExists =
            await context.ProgressRecords.AnyAsync(
                p => p.ClientId == client.Id);

        if (!progressExists)
        {
            var now = DateTime.UtcNow;

            await context.ProgressRecords.AddRangeAsync(
                new ProgressRecord
                {
                    ClientId = client.Id,
                    Weight = 82.5m,
                    BodyFatPercentage = 23.5m,
                    MuscleMass = 34m,
                    RecordedAt = now.AddDays(-28),
                    Notes = "Initial measurement"
                },
                new ProgressRecord
                {
                    ClientId = client.Id,
                    Weight = 81.5m,
                    BodyFatPercentage = 23m,
                    MuscleMass = 34.2m,
                    RecordedAt = now.AddDays(-14),
                    Notes = "Second measurement"
                },
                new ProgressRecord
                {
                    ClientId = client.Id,
                    Weight = 80.5m,
                    BodyFatPercentage = 22.5m,
                    MuscleMass = 34.4m,
                    RecordedAt = now,
                    Notes = "Latest measurement"
                });

            await context.SaveChangesAsync();
        }
    }

    private static async Task EnsureRoleAsync(
        RoleManager<IdentityRole> roleManager,
        string role)
    {
        if (await roleManager.RoleExistsAsync(role))
            return;

        var result = await roleManager.CreateAsync(
            new IdentityRole(role));

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ",
                    result.Errors.Select(e => e.Description)));
        }
    }

    private static async Task<ApplicationUser> EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string requiredRole)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                user,
                password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        createResult.Errors.Select(e => e.Description)));
            }
        }

        // Keep the seeded account in its intended role only.
        var currentRoles = await userManager.GetRolesAsync(user);

        var rolesToRemove = currentRoles
            .Where(r => r != requiredRole)
            .ToArray();

        if (rolesToRemove.Length > 0)
        {
            var removeResult = await userManager.RemoveFromRolesAsync(
                user,
                rolesToRemove);

            if (!removeResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        removeResult.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, requiredRole))
        {
            var addResult = await userManager.AddToRoleAsync(
                user,
                requiredRole);

            if (!addResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        addResult.Errors.Select(e => e.Description)));
            }
        }

        return user;
    }
}