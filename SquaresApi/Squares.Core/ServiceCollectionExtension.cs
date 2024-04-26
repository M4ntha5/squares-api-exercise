using Microsoft.Extensions.DependencyInjection;
using Squares.Core.Points;
using Squares.Core.Squares;
using Validation;

namespace Squares.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidators(typeof(ServiceCollectionExtension).Assembly);
        serviceCollection.AddValidation();
        serviceCollection.AddPointsServices();
        serviceCollection.AddSquaresServices();
        
        return serviceCollection;
    }
}