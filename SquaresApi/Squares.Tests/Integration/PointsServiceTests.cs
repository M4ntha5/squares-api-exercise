using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Squares.Core.Points.Dtos;
using Squares.Core.Points.Exceptions;
using Squares.Core.Points.Services.Interfaces;
using Squares.Data.Contexts;
using Squares.Data.Models;

namespace Squares.Tests.Integration;

public class PointsServiceTests : IClassFixture<BaseFixture>, IDisposable
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IPointsService _pointsService;

    public PointsServiceTests(BaseFixture baseFixture)
    {
        var serviceProvider = baseFixture.BuildAndGetServiceProvider();
        _pointsService = serviceProvider.GetRequiredService<IPointsService>();
        _applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        _applicationDbContext.Database.BeginTransaction();
    }


    public void Dispose()
    {
        _applicationDbContext.Database.RollbackTransaction();
    }

    [Fact]
    public async Task GetByIdShouldSuccessfullyFindPoint()
    {
        var point = new Point(5, 5);
        await _applicationDbContext.Points.AddAsync(point);
        await _applicationDbContext.SaveChangesAsync();
        
        var result =await _pointsService.GetById(point.Id);
        
        Assert.Equal(point.X, result.X);
        Assert.Equal(point.Y, result.Y);
        
    }
    
    [Fact]
    public async Task GetByIdShouldThrowWhenPointNotFound()
    {
        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await _pointsService.GetById(4));
    }
    
    [Fact]
    public async Task GetAllShouldReturnEmpty()
    {
        var result =await _pointsService.GetAll();
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetAllShouldReturn2Points()
    {
        var point1 = new Point(5, 5);
        var point2 = new Point(1, 2);
        await _applicationDbContext.Points.AddRangeAsync(new []{point1, point2});
        await _applicationDbContext.SaveChangesAsync();
        
        var result =await _pointsService.GetAll();
        Assert.Equal(2, result.Count);
    }
    
    
    [Fact]
    public async Task CreateShouldCreateSinglePoint()
    {
        var dto = new PointDto(5, 5);
        
        await _pointsService.Create(dto);

        var result = await _applicationDbContext.Points.FirstAsync(e => e.X == dto.X && e.Y == dto.Y);
        
        Assert.Equal(dto.X, result.X);
        Assert.Equal(dto.Y, result.Y);
    }
    
    [Fact]
    public async Task CreateShouldThrowWhenPointExist()
    {
        var point1 = new Point(5, 5);
        await _applicationDbContext.Points.AddAsync(point1);
        await _applicationDbContext.SaveChangesAsync();

        var result = await Assert.ThrowsAsync<PointsException>(async () => await _pointsService.Create(new PointDto(point1.X, point1.Y)));
        Assert.Equal(PointsException.PointAlreadyExist().Message, result.Message);
    }
    
    [Fact]
    public async Task DeleteShouldThrowWhenNoSuchPointToRemove()
    {
        var result = await Assert.ThrowsAsync<PointsException>(async () => await _pointsService.Delete(new PointDto(3, 2)));
        Assert.Equal(PointsException.NoPointToRemove().Message, result.Message);
    }
    
    [Fact]
    public async Task DeleteShouldSuccessfullyDeleteGivenPoint()
    {
        var point1 = new Point(5, 5);
        await _applicationDbContext.Points.AddAsync(point1);
        await _applicationDbContext.SaveChangesAsync();


        var dto = new PointDto(point1.X, point1.Y);

        await _pointsService.Delete(dto);

        var point = await _applicationDbContext.Points.FirstOrDefaultAsync(p => p.Id == point1.Id);
            
        Assert.Null(point);
    }
    
}