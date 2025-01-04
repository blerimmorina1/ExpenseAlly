using ExpenseAlly.Application.Features.Budgets.Commands;
using FluentValidation;

namespace ExpenseAlly.Application.Features.Budgets.Validators;

public class EditBudgetCommandValidator : AbstractValidator<EditBudgetCommand>
{
    public EditBudgetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Budget ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Budget name is required.")
            .MaximumLength(255).WithMessage("Budget name cannot exceed 255 characters.");

        RuleFor(x => x.TotalLimit)
            .GreaterThan(0).WithMessage("Total limit must be greater than zero.");

        RuleFor(x => x.StartDate)
           .NotEmpty().WithMessage("Start date is required.");
           

        RuleFor(x => x.BudgetDetails)
            .NotEmpty().WithMessage("At least one budget detail is required.")
            .Must(details => details != null && details.All(d => d.Limit > 0))
            .WithMessage("Each budget detail must have a limit greater than zero.")
            .ForEach(detail =>
            {
                detail.SetValidator(new BudgetDetailDtoValidator());
            });
    }
}