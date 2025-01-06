using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Application.Strategies;

public class BudgetDetailNotificationStrategy : INotificationStrategy
{
    public Notification GenerateNotification(object entity)
    {
        var budgetDetail = entity as BudgetDetail;
        if (budgetDetail == null) throw new ArgumentException("Invalid entity type");

        decimal percentageUsed = (budgetDetail.Spent / budgetDetail.Limit) * 100;

        if (percentageUsed > 100)
        {
            return new Notification
            {
                Type = NotificationType.BudgetDetail,
                Title = "Budget Overspent",
                Message = $"You have overspent your '{budgetDetail.Budget.Name}' budget for category '{budgetDetail.Category.Name}'.",
            };
        }
        else if (percentageUsed >= 80 && percentageUsed < 100)
        {
            decimal remaining = 100 - percentageUsed;
            return new Notification
            {
                Type = NotificationType.BudgetDetail,
                Title = "Budget Almost Completed",
                Message = $"Your '{budgetDetail.Budget.Name}' budget for category '{budgetDetail.Category.Name}' is almost complete. Only {remaining.ToString("0.00")}% remaining.",
            };
        }

        return null;
    }
}