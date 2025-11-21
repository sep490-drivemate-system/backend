using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Repositories.Models;

namespace ResourceService.Repositories.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answer");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.AttemptId)
                .HasColumnName("attempt_id")
                .IsRequired();

            builder.Property(x => x.ChoiceId)
                .HasColumnName("choice_id")
                .IsRequired();

            builder.Property(x => x.IsCorrect)
                .HasColumnName("is_correct")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");
        }
    }
}

