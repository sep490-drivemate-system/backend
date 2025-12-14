using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
{
    public class PostVideoConfiguration : IEntityTypeConfiguration<PostVideo>
    {
        public void Configure(EntityTypeBuilder<PostVideo> builder)
        {
            builder.ToTable("PostVideos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.PostId)
                .HasColumnName("post_id")
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnName("url")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}

