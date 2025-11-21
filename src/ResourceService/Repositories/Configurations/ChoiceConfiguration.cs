using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Repositories.Models;

namespace ResourceService.Repositories.Configurations
{
    public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
    {
        public void Configure(EntityTypeBuilder<Choice> builder)
        {
            builder.ToTable("Choice");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.QuestionId)
                .HasColumnName("question_id")
                .IsRequired();

            builder.Property(x => x.ChoiceText)
                .HasColumnName("choice_text")
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.IsCorrect)
                .HasColumnName("is_correct")
                .IsRequired()
                .HasDefaultValue(false);

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
                .WithOne(a => a.Choice)
                .HasForeignKey(a => a.ChoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

