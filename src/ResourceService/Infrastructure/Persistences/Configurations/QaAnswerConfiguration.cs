using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
{
    public class QaAnswerConfiguration : IEntityTypeConfiguration<QaAnswer>
    {
        public void Configure(EntityTypeBuilder<QaAnswer> builder)
        {
            builder.ToTable("QaAnswers");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.QuestionId)
                .HasColumnName("question_id")
                .IsRequired();

            builder.Property(x => x.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();

            builder.Property(x => x.Content)
                .HasColumnName("content")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.IsAccepted)
                .HasColumnName("is_accepted")
                .HasDefaultValue(false);

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
        }
    }
}

