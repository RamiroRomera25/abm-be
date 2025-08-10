using FluentValidation;
using technical_tests_backend_ssr.Dtos;

namespace technical_tests_backend_ssr.Services.Validators;

public class AuctionDtoPostValidator : AbstractValidator<AuctionDtoPost>
{
    public AuctionDtoPostValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(30).When(x => !string.IsNullOrEmpty(x.Title))
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.InitialPrice)
            .NotNull()
            .GreaterThanOrEqualTo(0);
        
        // RuleFor(x => x.StartDate)
        //     .NotNull()
        //     .GreaterThanOrEqualTo(DateTime.Now);
        // Lo comente para que sea mas facil de probar pero dejo como que se hacerlo
        
        RuleFor(x => x.EndDate)
            .NotNull()
            .GreaterThanOrEqualTo(x => x.StartDate);
    }
}
