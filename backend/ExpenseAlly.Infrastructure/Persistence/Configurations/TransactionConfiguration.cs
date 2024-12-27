using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        // Table configuration
        builder.ToTable("Transactions", t => t.HasComment("This entity is used to store all the transactions of user added to the system."));

        // Primary key configuration
        builder.HasKey(x => x.Id).HasName("PK_Transactions_Id");

        // Property configurations
        builder.Property(x => x.Type)
            .IsRequired()
            .HasComment("The type of transaction, such as Income or Expense.");

        builder.Property(x => x.CategoryId)
            .IsRequired()
            .HasComment("The ID of the category associated with this transaction.");

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasComment("The amount of money involved in the transaction.");

        builder.Property(x => x.Date)
            .IsRequired()
            .HasComment("The date when the transaction occurred.");

        builder.Property(x => x.Notes)
            .HasMaxLength(500)
            .HasComment("Additional notes or description for the transaction.");

        // Relationships
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Transactions_Category");
    }
}