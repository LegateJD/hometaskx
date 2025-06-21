using FluentValidation;

namespace HomeTask1.Users.WebApi.Validators;

public class UpdateUserValidator : AbstractValidator<Contracts.V1.UpdateUser>
{
    public UpdateUserValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.SubscriptionId)
            .GreaterThan(0).WithMessage("Subscription ID must be a positive integer.");
    }

}