using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Validation;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ValidatorsAdapter>();

        return serviceCollection;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var type = typeof(IValidator);

        var types = assembly.GetTypes()
            .Where(p => type.IsAssignableFrom(p) && p is { IsClass: true, IsAbstract: false });

        foreach (var validatorType in types) services.AddTransient(typeof(IValidator), validatorType);

        return services;
    }
}