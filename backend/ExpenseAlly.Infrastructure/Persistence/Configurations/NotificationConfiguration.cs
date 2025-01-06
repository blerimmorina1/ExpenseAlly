using ExpenseAlly.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAlly.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Table configuration
        builder.ToTable("Notifications", t => t.HasComment("This entity is used to store all notifications."));

        // Primary key configuration
        builder.HasKey(n => n.Id).HasName("PK_Notifications_Id");

        // Property configurations
        builder.Property(n => n.Type)
            .IsRequired()
            .HasComment("The type of the notification.");

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("The title of the notification.");

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(1000)
            .HasComment("The message content of the notification.");

        builder.Property(n => n.IsRead)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indicates whether the notification has been read.");
    }
}
