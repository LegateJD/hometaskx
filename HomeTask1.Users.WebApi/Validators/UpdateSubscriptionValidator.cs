using FluentValidation;
using HomeTask1.Users.Domain;

namespace HomeTask1.Users.WebApi.Validators;

public class UpdateSubscriptionValidator : AbstractValidator<Contracts.V1.UpdateSubscription>

{
    public UpdateSubscriptionValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .Must(type => Enum.TryParse(typeof(SubscriptionType), type, true, out _))
            .WithMessage("Invalid type. Valid types are: Free, Trial, Super.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate.");
    }
}