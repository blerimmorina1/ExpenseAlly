using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Strategies;
using ExpenseAlly.Application.Strategies;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Application.Common.Factories;

public class NotificationStrategyFactory
{
    public INotificationStrategy GetStrategy(NotificationType type)
    {
        return type switch
        {
            NotificationType.Budget => new BudgetNotificationStrategy(),
            NotificationType.SavingGoal => new SavingGoalNotificationStrategy(),
            NotificationType.BudgetDetail => new BudgetDetailNotificationStrategy(),
            _ => throw new ArgumentException("Invalid notification type")
        };
    }
}
