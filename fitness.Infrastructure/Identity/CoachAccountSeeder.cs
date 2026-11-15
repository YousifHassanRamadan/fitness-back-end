using fitness.Domain.Entities;
using fitness.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace fitness.Infrastructure.Identity;

public static class CoachAccountSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        var context =
            services.GetRequiredService<FitnessDbContext>();

        // Existing User Secrets
        var email = configuration["SeedAdmin:Email"];
        var password = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Coach account credentials are missing.");
        }

        const string role = "Coach";

        // Ensure Coach role exists
        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(
                new IdentityRole(role));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }

        // Find existing account
        var user = await userManager.FindByEmailAsync(email);

        // Create account if it does not exist
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user,
                password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }
        }

        // Ensure this account has Coach role only
        var currentRoles = await userManager.GetRolesAsync(user);

        var otherRoles = currentRoles
            .Where(r => r != role)
            .ToArray();

        if (otherRoles.Length > 0)
        {
            var removeResult = await userManager.RemoveFromRolesAsync(
                user,
                otherRoles);

            if (!removeResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        removeResult.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var addResult = await userManager.AddToRoleAsync(user, role);

            if (!addResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        addResult.Errors.Select(e => e.Description)));
            }
        }

        // Create Coach profile if it does not exist
        var coach = await context.Coaches
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (coach == null)
        {
            coach = new Coach
            {
                UserId = user.Id,
                FirstName = configuration["SeedAdmin:FirstName"] ?? "Default",
                LastName = configuration["SeedAdmin:LastName"] ?? "Coach",
                Bio = "Seeded coach account"
            };

            await context.Coaches.AddAsync(coach);
            await context.SaveChangesAsync();
        }
    }
}