using FluentValidation;
using technical_tests_backend_ssr.Dtos;

namespace technical_tests_backend_ssr.Services.Validators;

public class AuctionDtoPutValidator : AbstractValidator<AuctionDtoPut>
{
    public AuctionDtoPutValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(30).When(x => !string.IsNullOrEmpty(x.Title))
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.StartDate)
            .NotNull()
            .GreaterThanOrEqualTo(DateTime.Now);
        
        RuleFor(x => x.EndDate)
            .NotNull()
            .GreaterThanOrEqualTo(x => x.StartDate);
    }
}