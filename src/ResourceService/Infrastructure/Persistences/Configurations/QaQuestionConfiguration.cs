using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
{
    public class QaQuestionConfiguration : IEntityTypeConfiguration<QaQuestion>
    {
        public void Configure(EntityTypeBuilder<QaQuestion> builder)
        {
            builder.ToTable("QaQuestions");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Content)
                .HasColumnName("content")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasMany(x => x.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

