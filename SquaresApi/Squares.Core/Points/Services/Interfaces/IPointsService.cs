using Squares.Core.Points.Dtos;

namespace Squares.Core.Points.Services.Interfaces;

public interface IPointsService
{
    Task Create(PointDto request);
    Task Create(CreatePointsBulkDto request);
    Task<ICollection<PointDto>> GetAll();
    Task<PointDto> GetById(int id);
    Task Delete(PointDto request);
}