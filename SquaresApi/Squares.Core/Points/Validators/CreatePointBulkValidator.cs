using FluentValidation;
using Squares.Core.Points.Dtos;

namespace Squares.Core.Points.Validators;

public class CreatePointBulkValidator : AbstractValidator<CreatePointsBulkDto>
{
    public CreatePointBulkValidator()
    {
        RuleFor(e => e.Points).Must(e => e.Count > 0);
    }
}