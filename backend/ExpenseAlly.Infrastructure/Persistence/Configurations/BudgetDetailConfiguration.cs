using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class BudgetDetailConfiguration : IEntityTypeConfiguration<BudgetDetail>
{
    public void Configure(EntityTypeBuilder<BudgetDetail> builder)
    {
        // Table configuration
        builder.ToTable("BudgetDetails", t => t.HasComment("This entity is used to store details of budgets, including category limits."));

        // Primary key configuration
        builder.HasKey(x => x.Id).HasName("PK_BudgetDetails_Id");

        // Property configurations
        builder.Property(x => x.BudgetId)
            .IsRequired()
            .HasComment("The ID of the associated budget.");

        builder.Property(x => x.CategoryId)
            .IsRequired()
            .HasComment("The ID of the associated transaction category.");

        builder.Property(x => x.Limit)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasComment("The spending limit for this category within the budget.");

        builder.Property(x => x.Spent)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasComment("The amount spent for this category within the budget.");

        // Relationships
        builder.HasOne(x => x.Budget)
            .WithMany(x => x.BudgetDetails)
            .HasForeignKey(x => x.BudgetId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_BudgetDetails_Budgets");

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BudgetDetails_TransactionCategories");
    }
}