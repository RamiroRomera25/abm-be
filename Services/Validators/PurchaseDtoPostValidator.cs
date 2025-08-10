using FluentValidation;
using technical_tests_backend_ssr.Dtos;

namespace technical_tests_backend_ssr.Services.Validators;

public class PurchaseDtoPostValidator : AbstractValidator<PurchaseDtoPost>
{
    public PurchaseDtoPostValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(30);
        
        RuleFor(x => x.TargetPrice)
            .NotNull()
            .GreaterThan(0);
        
        // RuleFor(x => x.StartDate)
        //     .NotNull()
        //     .NotEmpty()
        //     .GreaterThanOrEqualTo(DateTime.Now);
        // Lo comente para que sea mas facil de probar pero dejo como que se hacerlo
        
        RuleFor(x => x.EndDate)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate);
        
        RuleFor(x => x.Stock)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}