using AutoMapper;
using Squares.Core.Points.Dtos;
using Squares.Data.Models;

namespace Squares.Core.Points.AutoMapperProfiles;

public class PointsProfile : Profile
{
    public PointsProfile()
    {
        CreateMap<PointDto, Point>();
        CreateMap<Point, PointDto>();
    }
}