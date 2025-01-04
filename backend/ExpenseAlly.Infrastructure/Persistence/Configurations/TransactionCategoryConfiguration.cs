using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class TransactionCategoryConfiguration : IEntityTypeConfiguration<TransactionCategory>
{
    public void Configure(EntityTypeBuilder<TransactionCategory> builder)
    {
        // Table configuration
        builder.ToTable("TransactionCategories", t => t.HasComment("This entity is used to store all categories for transactions."));

        // Primary key configuration
        builder.HasKey(x => x.Id).HasName("PK_TransactionCategories_Id");

        // Property configurations
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("The name of the transaction category.");

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .HasComment("Optional description for the transaction category.");

        builder.Property(x => x.Type)
            .IsRequired()
            .HasComment("The type of transaction associated with this category, such as Income or Expense.");

    }
}
