namespace Squares.Core.Points.Exceptions;

public class PointsException : Exception
{
    private PointsException(string message) : base(message)
    {
    }
    
    public static PointsException NoPointToRemove() => new("No point to remove");
    public static PointsException PointAlreadyExist() => new("This point already exist");
}