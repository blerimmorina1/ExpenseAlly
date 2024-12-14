using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions", t => t.HasComment("This entity is used to store all the transactions of user added to the system ."));

        builder.HasKey(x => x.Id).HasName("PK_Transactions_Id");
    }
}
