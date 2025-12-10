using MessagingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingService.Infrastructure.Persistence.Configurations
{
    public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
    {
        public void Configure(EntityTypeBuilder<ChatSession> builder)
        {
            builder.ToTable("ChatSession");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(c => c.InstructorId)
                .HasColumnName("instructor_id")
                .IsRequired();

            builder.Property(c => c.NoviceDriverId)
                .HasColumnName("novice_driver_id")
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(c => c.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(c => c.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships
            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

