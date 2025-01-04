using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class SavingGoalConfiguration : IEntityTypeConfiguration<SavingGoal>
{
    public void Configure(EntityTypeBuilder<SavingGoal> builder)
    {
        builder.ToTable("SavingGoals", t => t.HasComment("This entity is used to store all the saving goals of users."));
        
        builder.HasKey(x => x.Id).HasName("PK_SavingGoals_Id");

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.TargetAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CurrentAmount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.Deadline)
            .IsRequired(false);

        builder.Property(x => x.IsCompleted)
            .HasDefaultValue(false);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);
    }
}