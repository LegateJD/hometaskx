using FluentValidation;

namespace HomeTask1.Users.WebApi.Validators;

public class AddUserValidator : AbstractValidator<Contracts.V1.CreateUser>
{
    public AddUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        
        RuleFor(x => x.SubscriptionId)
            .GreaterThan(0).WithMessage("Subscription ID must be a valid, positive integer.");
    }
}
