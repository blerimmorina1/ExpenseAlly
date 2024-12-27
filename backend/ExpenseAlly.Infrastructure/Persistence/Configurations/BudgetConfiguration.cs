using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        // Table configuration
        builder.ToTable("Budgets", t => t.HasComment("This entity is used to store user budgets."));

        // Primary key configuration
        builder.HasKey(x => x.Id).HasName("PK_Budgets_Id");

        // Property configurations
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("The name of the budget.");

        builder.Property(x => x.TotalLimit)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasComment("The total spending limit for the budget.");

        builder.Property(x => x.TotalSpent)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasComment("The total amount spent of the budget.");

        builder.Property(x => x.StartDate)
            .IsRequired()
            .HasComment("The start date of the budget period.");

        builder.Property(x => x.EndDate)
            .IsRequired()
            .HasComment("The end date of the budget period.");

    }
}