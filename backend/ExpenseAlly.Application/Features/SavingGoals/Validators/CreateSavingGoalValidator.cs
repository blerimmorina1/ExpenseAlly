using FluentValidation;
using ExpenseAlly.Application.Features.SavingGoals.Commands;

namespace ExpenseAlly.Application.Features.SavingGoals.Validators;

public class CreateSavingGoalValidator : AbstractValidator<CreateSavingGoalCommand>
{
    public CreateSavingGoalValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

        RuleFor(x => x.TargetAmount)
            .GreaterThan(0).WithMessage("Target amount must be greater than zero.");

        RuleFor(x => x.CurrentAmount)
            .LessThanOrEqualTo(x=> x.TargetAmount).WithMessage("Current amount can't be greater than target amount.");

        RuleFor(x => x.Deadline)
            .Must(BeAValidDate).WithMessage("Invalid deadline.")
            .GreaterThan(DateTime.Now).When(x => x.Deadline.HasValue)
            .WithMessage("Deadline must be a future date.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
    }

    private bool BeAValidDate(DateTime? date)
    {
        return !date.HasValue || date.Value > DateTime.MinValue;
    }
}