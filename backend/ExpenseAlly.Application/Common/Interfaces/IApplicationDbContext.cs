using ExpenseAlly.Domain.Entities;

namespace ExpenseAlly.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Transaction> Transactions { get; }
    DbSet<TransactionCategory> TransactionCategories { get; }
    DbSet<Budget> Budgets { get; }
    DbSet<BudgetDetail> BudgetDetails { get; }

    DbSet<SavingGoal> SavingGoals { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
