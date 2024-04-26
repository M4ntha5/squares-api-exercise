using Microsoft.AspNetCore.Mvc;
using Squares.Core.Points.Dtos;
using Squares.Core.Points.Services.Interfaces;
using SquaresApi.Controllers.Base;

namespace SquaresApi.Controllers;

public class PointsController : BaseV1ApiController
{
    private readonly IPointsService _pointsService;

    public PointsController(IPointsService pointsService)
    {
        _pointsService = pointsService;
    }
    
    [HttpGet]
    public async Task<ICollection<PointDto>> GetAll()
    {
       return await _pointsService.GetAll();
    }
    
    [HttpGet("{id:int}")]
    public async Task<PointDto> GetById(int id)
    {
        return await _pointsService.GetById(id);
    }
    
    [HttpPost]
    public async Task Create([FromBody]PointDto request)
    {
        await _pointsService.Create(request);
    }

    [HttpPost("bulk")]
    public async Task CreateBulk([FromBody] CreatePointsBulkDto request)
    {
        await _pointsService.Create(request);
    }
    
    [HttpDelete]
    public async Task Delete([FromBody] PointDto request)
    {
        await _pointsService.Delete(request);
    }
    
}