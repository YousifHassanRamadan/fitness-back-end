using fitness.Application.Mapping;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace fitness.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()));

        // FluentValidation
        services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly());

        services.AddAutoMapper(cfg => { },
      typeof(CoachMappingProfile));

        return services;
    }
}