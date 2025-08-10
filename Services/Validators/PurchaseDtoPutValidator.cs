using FluentValidation;
using technical_tests_backend_ssr.Dtos;

namespace technical_tests_backend_ssr.Services.Validators;

public class PurchaseDtoPutValidator : AbstractValidator<PurchaseDtoPut>
{
    public PurchaseDtoPutValidator()
    {
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.StartDate)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now);

        RuleFor(x => x.EndDate)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate);

        RuleFor(x => x.Stock)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}