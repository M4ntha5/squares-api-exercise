using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Squares.Core.Points;
using Squares.Data.Contexts;

namespace Squares.Tests;

public class BaseFixture : IDisposable
{
    private static readonly object Lock = new object();
    private static bool _databaseInitialized;

    private ServiceCollection ServiceCollection { get; }

    public ServiceProvider BuildAndGetServiceProvider(Action<ServiceCollection> options)
    {
        options.Invoke(ServiceCollection);
        return ServiceCollection.BuildServiceProvider();
    }

    public ServiceProvider BuildAndGetServiceProvider()
    {
        return ServiceCollection.BuildServiceProvider();
    }

    public BaseFixture()
    {
        ServiceCollection = new ServiceCollection();
        ServiceCollection.AddLogging();
        Core.ServiceCollectionExtension.AddCoreServices(ServiceCollection);
            
        ServiceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql("User ID=postgres;Password=postgres;Server=localhost;Port=10432;Database=adform-testing;Integrated Security=true;Pooling=true;Include Error Detail=true;",
                config => config.MigrationsAssembly("Squares.Data.Migrations"));
        });

        ServiceCollection.AddAutoMapper(action =>
        {
            action.AddMaps(typeof(ServiceCollectionExtension).Assembly);
        });

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        lock (Lock)
        {
            if (_databaseInitialized) return;

            var provider = ServiceCollection.BuildServiceProvider();
            var dbContext = provider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            _databaseInitialized = true;
        }
    }

    public void Dispose()
    {
    }
}