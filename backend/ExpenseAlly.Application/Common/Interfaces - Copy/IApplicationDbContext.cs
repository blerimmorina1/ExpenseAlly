using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAlly.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Transaction> Transactions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
