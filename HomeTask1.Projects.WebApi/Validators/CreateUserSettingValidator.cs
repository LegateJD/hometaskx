using FluentValidation;
using HomeTask1.Projects.Domain.Entities;

namespace HomeTask1.Projects.WebApi.Validators;

public class CreateUserSettingValidator : AbstractValidator<Contracts.V1.CreateUserSetting>
{
    public CreateUserSettingValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than zero.");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.")
            .Must(type => Enum.TryParse(typeof(Language), type, true, out _))
            .WithMessage("Language must be either 'English' or 'Spanish'.");

        RuleFor(x => x.Theme)
            .NotEmpty().WithMessage("Theme is required.")
            .Must(type => Enum.TryParse(typeof(Theme), type, true, out _))
            .WithMessage("Theme must be either 'Light' or 'Dark'.");
    }
}
