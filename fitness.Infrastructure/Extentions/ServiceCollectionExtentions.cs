using fitness.Application.Interfaces;
using fitness.Infrastructure.Identity;
using fitness.Infrastructure.Persistence;
using fitness.Infrastructure.Repositories;
using fitness.Infrastructure.Services;
using fitness.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace fitness.Infrastructure.Extentions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<FitnessDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        // Identity
        services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<FitnessDbContext>()
            .AddDefaultTokenProviders();


        //Scoped Services

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICoachManagementRepository,CoachManagementService>();
        services.AddScoped<IClientManagementRepository,ClientManagementService>();


        ///JWT Settings
        ///
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));





        return services;
    }


    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "Coach", "Client" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}