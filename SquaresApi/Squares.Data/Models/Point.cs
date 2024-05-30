using Squares.Data.Base;

namespace Squares.Data.Models;

public class Point : IdBasedEntity
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    //test
   
    // distance from current point to given point
    public int Distance(Point point)
    {
        return (X - point.X) * (X - point.X) + (Y - point.Y) * (Y - point.Y);
    }
}