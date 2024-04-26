using Microsoft.Extensions.DependencyInjection;
using Squares.Core.Squares.Services.Interfaces;
using Squares.Data.Contexts;
using Squares.Data.Models;

namespace Squares.Tests.Integration;

public class SquaresServiceTests : IClassFixture<BaseFixture>, IDisposable
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ISquaresService _squaresService;

    public SquaresServiceTests(BaseFixture baseFixture)
    {
        var serviceProvider = baseFixture.BuildAndGetServiceProvider();
        _squaresService = serviceProvider.GetRequiredService<ISquaresService>();
        _applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        _applicationDbContext.Database.BeginTransaction();
    }


    public void Dispose()
    {
        _applicationDbContext.Database.RollbackTransaction();
    }
    
    private async Task SeedPointsWithoutSquares()
    {
        var points = new List<Point>
        {
            new Point(-3, 14),
            new Point(5, -9),
            new Point(-2, 7),
            new Point(0, 3),
            new Point(2, 0),
            new Point(1, 2),
            new Point(-4, -3),
            new Point(-1, 5),
            new Point(4, -2),
            new Point(3, 2)
        };

        await _applicationDbContext.Points.AddRangeAsync(points);
        await _applicationDbContext.SaveChangesAsync();
    }
    
    private async Task SeedPointsWithSquares()
    {
        var points = new List<Point>
        {
            new Point(1, 1),
            new Point(1, -1),
            new Point(-1, 1),
            new Point(-1, -1),
            new Point(4, 4),
            new Point(5, 20),
            new Point(10, 20),
            new Point(10, 15),
            new Point(5, 15),
        };
        await _applicationDbContext.Points.AddRangeAsync(points);
        await _applicationDbContext.SaveChangesAsync();
    }
    
    
    [Fact]
    public async Task FindSquaresShouldFindSquares()
    {
        await SeedPointsWithSquares();
        
        var result = await _squaresService.FindSquares();
        
       Assert.NotEmpty(result);
       Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task FindSquaresShouldNotFindAnySquares()
    {
        await SeedPointsWithoutSquares();
        
        var result = await _squaresService.FindSquares();
        
        Assert.Empty(result);
    }

}