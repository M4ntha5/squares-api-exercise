using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Squares.Core.Points.Dtos;
using Squares.Core.Squares.Dtos;
using Squares.Core.Squares.Services.Interfaces;
using Squares.Data.Contexts;
using Squares.Data.Models;

namespace Squares.Core.Squares.Services;

public class SquaresService : ISquaresService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public SquaresService(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    
    //TODO improve to find rotated squares
    //TODO improve to find more than 1 square from a single point
    public async Task<ICollection<SquareDto>> FindSquares()
    {
        var points = await _applicationDbContext.Points.ToListAsync();

        var usedPoints = new List<Point>();
        var squares = new List<SquareDto>();

        foreach (var point1 in points)
        {
            if (usedPoints.Any(p => p.Id == point1.Id))
            {
                continue;
            }
            
            foreach (var point2 in points)
            {
                if (usedPoints.Any(p => p.Id == point2.Id))
                {
                    continue;
                }

                var (expectedPoint3, expectedPoint4) = FindMissingVertices(point1, point2);
                if (expectedPoint3 == null || expectedPoint4 == null)
                {
                    continue;
                }

                var point3 = points.FirstOrDefault(p => p.X == expectedPoint3.X && p.Y == expectedPoint3.Y);
                var point4 = points.FirstOrDefault(p => p.X == expectedPoint4.X && p.Y == expectedPoint4.Y);

                if (point3 == null || point4 == null)
                {
                    continue;
                }

                if (!IsSquare(point1, point2, point3, point4))
                {
                    continue;
                }

                squares.Add(new SquareDto(
                    _mapper.Map<PointDto>(point1), _mapper.Map<PointDto>(point2),
                    _mapper.Map<PointDto>(point3), _mapper.Map<PointDto>(point4)));
                usedPoints.AddRange(new []{point1, point2, point3, point4});
            }
        }

        return squares;
    }

    private static (Point? point1, Point? point2) FindMissingVertices(Point p1, Point p2)
    {
        Point? point3, point4;
        // Check if the x-coordinates are equal
        if (p1.X == p2.X && p1.Y == p2.Y)
        {
            return (null, null);
        }
        
        if (p1.X == p2.X)
        {
            point3 = new Point(p1.X + p2.Y - p1.Y, p1.Y);
            point4 = new Point(p2.X + p2.Y - p1.Y, p2.Y);
            return (point3, point4);
        }
    
        // Check if the y-coordinates are equal
        if (p1.Y == p2.Y)
        {
            point3 = new Point(p1.X, p1.Y + p2.X - p1.X);
            point4 = new Point(p2.X, p2.Y + p2.X - p1.X);
            return (point3, point4);
        }
    
        // If the given coordinates forms a diagonal of the square
        if (Math.Abs(p2.X - p1.X) == Math.Abs(p2.Y - p1.Y))
        {
            point3 = new Point(p1.X, p1.Y + p2.X - p1.X);
            point4 = new Point(p2.X, p2.Y + p2.X - p1.X);
            return (point3, point4);
        }
    
        // Otherwise Square does not exist
        return (null, null);
        
    }

    //algorithm from: https://www.geeksforgeeks.org/check-given-four-points-form-square/
    private static bool IsSquare(Point p1, Point p2, Point p3, Point p4)
    {
        var dist2 = p1.Distance(p2);
        var dist3 = p1.Distance(p3);
        var dist4 = p1.Distance(p4);
        
        if (dist2 == dist3 && 2 * dist2 == dist4)
        {
            var dist = p2.Distance(p4);
            return dist == p3.Distance(p4) && dist == dist2;
        }
        
        if (dist3 == dist4 && 2 * dist3 == dist2)
        {
            var dist = p2.Distance(p3);
            return dist == p2.Distance(p4) && dist == dist3;
        }
    
        if (dist2 == dist4 && 2 * dist2 == dist3)
        {
            var dist = p2.Distance(p3);
            return dist == p3.Distance(p4) && dist == dist2;
        }
    
        return false;
    
    }

}