using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingService.Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(m => m.Content)
                .HasColumnName("content")
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(m => m.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(MessageStatus.Sent);


            builder.Property(m => m.ChatSessionId)
                .HasColumnName("chat_session_id")
                .IsRequired();

            builder.Property(m => m.SenderId)
                .HasColumnName("sender_id")
                .IsRequired();

            builder.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(m => m.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(m => m.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(m => m.ChatSessionId);
            builder.HasIndex(m => m.SenderId);
            builder.HasIndex(m => m.CreatedAt);
        }
    }
}

