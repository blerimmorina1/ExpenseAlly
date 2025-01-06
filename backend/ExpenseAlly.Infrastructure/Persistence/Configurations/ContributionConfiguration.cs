using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
{
    public void Configure(EntityTypeBuilder<Contribution> builder)
    {
        // Table name and comments
        builder.ToTable("Contributions", t => t.HasComment("Stores all contributions made towards saving goals."));

        // Primary Key
        builder.HasKey(c => c.Id).HasName("PK_Contributions_Id");

        // Properties
        builder.Property(c => c.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasComment("The contributed amount.");

        builder.Property(c => c.Date)
            .IsRequired()
            .HasComment("The date of the contribution.");

        // Relationships
        builder.HasOne(c => c.SavingGoal)
            .WithMany(g => g.Contributions)
            .HasForeignKey(c => c.SavingGoalId)
            .OnDelete(DeleteBehavior.Cascade) // Optional: Change to ClientSetNull if required.
            .IsRequired(false) // Allow null relationships.
            .HasConstraintName("FK_Contributions_SavingGoals");
    }
}