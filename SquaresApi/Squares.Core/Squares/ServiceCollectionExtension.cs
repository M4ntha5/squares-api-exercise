using Microsoft.Extensions.DependencyInjection;
using Squares.Core.Squares.Services;
using Squares.Core.Squares.Services.Interfaces;

namespace Squares.Core.Squares;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSquaresServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISquaresService, SquaresService>();

        return serviceCollection;
    }
}