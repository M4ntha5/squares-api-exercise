using FluentValidation;
using Squares.Core.Points.Dtos;

namespace Squares.Core.Points.Validators;

public class CreatePointValidator : AbstractValidator<PointDto>
{
    public CreatePointValidator()
    {
        RuleFor(e => e.X).NotEmpty();
        RuleFor(e => e.Y).NotEmpty();
    }
}