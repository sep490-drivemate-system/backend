using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Repositories.Models;

namespace ResourceService.Repositories.Configurations
{
    public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
    {
        public void Configure(EntityTypeBuilder<Attempt> builder)
        {
            builder.ToTable("Attempt");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.QuizId)
                .HasColumnName("quiz_id")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            // Relationships
            builder.HasMany(x => x.Answers)
                .WithOne(a => a.Attempt)
                .HasForeignKey(a => a.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

