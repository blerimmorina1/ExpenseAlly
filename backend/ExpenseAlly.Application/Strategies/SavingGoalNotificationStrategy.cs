using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using ExpenseAlly.Domain.Enums;
namespace ExpenseAlly.Application.Common.Strategies;


public class SavingGoalNotificationStrategy : INotificationStrategy
{
    public Notification GenerateNotification(object entity)
    {
        var savingsGoal = entity as SavingGoal; 
        if (savingsGoal == null) throw new ArgumentException("Invalid entity type");

        if (savingsGoal.CurrentAmount >= savingsGoal.TargetAmount)
        {
            return new Notification
            {
                Type = NotificationType.SavingGoal,
                Title = "Savings Goal Achieved",
                Message = $"Congratulations! You've achieved your savings goal for '{savingsGoal.Name}'.",
            };
        }

        return null;
    }
}
