using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingService.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(n => n.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Content)
                .HasColumnName("content")
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(n => n.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasConversion<int>();

            builder.Property(n => n.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(NotificationStatus.Unread);

            builder.Property(n => n.ActionUrl)
                .HasColumnName("action_url")
                .HasMaxLength(1000);


            builder.Property(n => n.UserId)
                .HasColumnName("user_id")
                .IsRequired();


            builder.Property(n => n.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(n => n.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(n => n.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(n => n.UserId);
            builder.HasIndex(n => new { n.UserId, n.Status });
            builder.HasIndex(n => n.CreatedAt);
        }
    }
}

