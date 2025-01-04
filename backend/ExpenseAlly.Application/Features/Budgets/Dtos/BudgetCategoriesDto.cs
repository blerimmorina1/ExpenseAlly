using ExpenseAlly.Application.Common.Models;

namespace ExpenseAlly.Application.Features.Budgets.Dtos
{
    public class BudgetCategoriesDto : ResponseDto
    {
        public List<BudgetCategoryDto> Categories { get; set; }
    }
}
