using ExpenseAlly.Domain.Entities;
using MediatR;

namespace ExpenseAlly.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Transaction> Transactions { get; }
    DbSet<TransactionCategory> TransactionCategories { get; }
    DbSet<Budget> Budgets { get; }
    DbSet<BudgetDetail> BudgetDetails { get; }
    DbSet<SavingGoal> SavingGoals { get; }
    DbSet<Notification> Notifications { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
