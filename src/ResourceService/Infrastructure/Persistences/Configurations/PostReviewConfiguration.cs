using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
{
    public class PostReviewConfiguration : IEntityTypeConfiguration<PostReview>
    {
        public void Configure(EntityTypeBuilder<PostReview> builder)
        {
            builder.ToTable("PostReviews");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.PostId)
                .HasColumnName("post_id")
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasColumnName("reason")
                .HasColumnType("text");

            builder.Property(x => x.ReviewerId)
                .HasColumnName("reviewer_id");

            builder.Property(x => x.ReviewedAt)
                .HasColumnName("reviewed_at")
                .HasColumnType("timestamp");

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

