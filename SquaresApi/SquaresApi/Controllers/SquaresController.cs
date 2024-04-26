using Microsoft.AspNetCore.Mvc;
using Squares.Core.Squares.Dtos;
using Squares.Core.Squares.Services.Interfaces;
using SquaresApi.Controllers.Base;

namespace SquaresApi.Controllers;

public class SquaresController : BaseV1ApiController
{
    private readonly ISquaresService _squaresService;

    public SquaresController(ISquaresService pointsService)
    {
        _squaresService = pointsService;
    }
    
    [HttpGet]
    public async Task<ICollection<SquareDto>> FindSquares()
    {
        return await _squaresService.FindSquares();
    }
    
}