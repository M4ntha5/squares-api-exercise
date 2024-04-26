using Squares.Core.Squares.Dtos;

namespace Squares.Core.Squares.Services.Interfaces;

public interface ISquaresService
{
    Task<ICollection<SquareDto>> FindSquares();

}