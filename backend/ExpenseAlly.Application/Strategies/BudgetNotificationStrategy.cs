using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Application.Common.Strategies;

public class BudgetNotificationStrategy : INotificationStrategy
{
    public Notification GenerateNotification(object entity)
    {
        var budget = entity as Budget;
        if (budget == null) throw new ArgumentException("Invalid entity type");

        decimal percentageUsed = (budget.TotalSpent / budget.TotalLimit) * 100;

        if (percentageUsed > 100)
        {
            return new Notification
            {
                Type = NotificationType.Budget,
                Title = "Budget Overspent",
                Message = $"You have overspent your '{budget.Name}' budget.",
            };
        }
        else if (percentageUsed >= 80 && percentageUsed < 100)
        {
            decimal remaining = 100 - percentageUsed;
            return new Notification
            {
                Type = NotificationType.Budget,
                Title = "Budget Almost Completed",
                Message = $"Your '{budget.Name}' budget is almost complete. Only {remaining.ToString("0.00")}% remaining.",
            };
        }

        return null;
    }
}
