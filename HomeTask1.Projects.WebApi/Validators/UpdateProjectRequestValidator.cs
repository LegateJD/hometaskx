using FluentValidation;

namespace HomeTask1.Projects.WebApi.Validators;

public class UpdateProjectRequestValidator : AbstractValidator<Contracts.V1.UpdateProject>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than zero.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters.");

        RuleForEach(x => x.Charts)
            .SetValidator(new UpdateChartRequestValidator());
    }
}

public class UpdateChartRequestValidator : AbstractValidator<Contracts.V1.UpdateChart>
{
    public UpdateChartRequestValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Chart symbol is required.")
            .MaximumLength(50).WithMessage("Symbol cannot exceed 50 characters.");

        RuleFor(x => x.Timeframe)
            .NotEmpty().WithMessage("Timeframe is required.")
            .MaximumLength(30).WithMessage("Timeframe cannot exceed 30 characters.");

        RuleForEach(x => x.Indicators)
            .SetValidator(new UpdateIndicatorRequestValidator());
    }
}

public class UpdateIndicatorRequestValidator : AbstractValidator<Contracts.V1.UpdateIndicator>
{
    public UpdateIndicatorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Indicator name is required.")
            .MaximumLength(100).WithMessage("Indicator name cannot exceed 100 characters.");

        RuleFor(x => x.Parameters)
            .MaximumLength(500).WithMessage("Indicator parameters cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Parameters));
    }
}
