using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Domain.Entities;

namespace ExpenseAlly.Application.Features.Budgets.Dtos
{
    public class BudgetCategoriesDto : ResponseDto
    {
        public List<BudgetCategoryDto> Categories { get; set; }
    }
}
