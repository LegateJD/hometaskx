using FluentValidation;

namespace HomeTask1.Projects.WebApi.Validators;

public class CreateProjectRequestValidator : AbstractValidator<Contracts.V1.CreateProject>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than zero.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters.");

        RuleForEach(x => x.Charts)
            .SetValidator(new CreateChartRequestValidator());
    }
}

public class CreateChartRequestValidator : AbstractValidator<Contracts.V1.CreateChart>
{
    public CreateChartRequestValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Chart symbol is required.")
            .MaximumLength(50).WithMessage("Symbol cannot exceed 50 characters.");

        RuleFor(x => x.Timeframe)
            .NotEmpty().WithMessage("Timeframe is required.")
            .MaximumLength(30).WithMessage("Timeframe cannot exceed 30 characters.");

        RuleForEach(x => x.Indicators)
            .SetValidator(new CreateIndicatorRequestValidator());
    }
}

public class CreateIndicatorRequestValidator : AbstractValidator<Contracts.V1.CreateIndicator>
{
    public CreateIndicatorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Indicator name is required.")
            .MaximumLength(100).WithMessage("Indicator name cannot exceed 100 characters.");

        RuleFor(x => x.Parameters)
            .MaximumLength(500).WithMessage("Indicator parameters cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Parameters));
    }
}
