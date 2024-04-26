using Microsoft.Extensions.DependencyInjection;
using Squares.Core.Points.Services;
using Squares.Core.Points.Services.Interfaces;

namespace Squares.Core.Points;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPointsServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IPointsService, PointsService>();

        return serviceCollection;
    }
}