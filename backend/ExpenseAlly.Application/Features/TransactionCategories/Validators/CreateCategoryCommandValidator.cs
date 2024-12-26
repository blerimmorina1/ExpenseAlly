using ExpenseAlly.Application.Features.TransactionCategories.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Validators
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(255)
                .WithMessage("Category name cannot exceed 255 characters.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Category type is required.");
        }
    }
}
