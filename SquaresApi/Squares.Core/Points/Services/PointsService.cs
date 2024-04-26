using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Squares.Core.Points.Dtos;
using Squares.Core.Points.Exceptions;
using Squares.Core.Points.Services.Interfaces;
using Squares.Data.Contexts;
using Squares.Data.Models;
using Validation;

namespace Squares.Core.Points.Services;

public class PointsService : IPointsService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ValidatorsAdapter _validatorsAdapter;
    private readonly IMapper _mapper;

    public PointsService(ApplicationDbContext applicationDbContext, ValidatorsAdapter validatorsAdapter, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _validatorsAdapter = validatorsAdapter;
        _mapper = mapper;
    }
    
    //TODO to improve performance maybe after each new point calculate possible squares in background and save it to DB
    public async Task Create(PointDto request)
    {
        await _validatorsAdapter.ValidateAndThrowAsync(request);

        if (await _applicationDbContext.Points.AnyAsync(p => p.X == request.X && p.Y == request.Y))
        {
            throw PointsException.PointAlreadyExist();
        }
        var point = _mapper.Map<Point>(request);

        await _applicationDbContext.AddAsync(point);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task Create(CreatePointsBulkDto request)
    {
        await _validatorsAdapter.ValidateAndThrowAsync(request);
        
        //TODO maybe think of some kind validation for duplicates
        var points = _mapper.Map<ICollection<Point>>(request.Points);
        
        await _applicationDbContext.AddRangeAsync(points);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<PointDto>> GetAll()
    {
        //TODO add pagination
        var points = await _applicationDbContext.Points.ToListAsync();
        return _mapper.Map<ICollection<PointDto>>(points);
    }

    public async Task<PointDto> GetById(int id)
    {
        var point = await _applicationDbContext.Points.FirstAsync(p => p.Id == id);
        return _mapper.Map<PointDto>(point);
    }

    public async Task Delete(PointDto request)
    {
        var point = await _applicationDbContext.Points.FirstOrDefaultAsync(p => p.Y == request.Y && p.X == request.X);
        if (point == null)
        {
            throw PointsException.NoPointToRemove();
        }

        _applicationDbContext.Remove(point);
        await _applicationDbContext.SaveChangesAsync();
    }
}