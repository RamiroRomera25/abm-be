using FluentValidation;
using Microsoft.VisualBasic.CompilerServices;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models.Enums;

namespace technical_tests_backend_ssr.Services.Validators;

public class MovementDtoPostValidator : AbstractValidator<MovementDtoPost>
{
    public MovementDtoPostValidator()
    {
        RuleFor(x => x.Cost)
            .GreaterThan(0);

        RuleFor(x => x.Comments)
            .MaximumLength(500);

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.MovementType)
            .IsInEnum();
    }
}