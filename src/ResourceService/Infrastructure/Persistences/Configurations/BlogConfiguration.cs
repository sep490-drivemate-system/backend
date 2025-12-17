using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            // table name
            builder.ToTable("Blog");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.CategoryId)
                   .HasColumnName("category_id")
                   .IsRequired(false);

            builder.Property(u => u.InstructorId)
                  .HasColumnName("instructor_id")
                  .IsRequired();

            builder.Property(u => u.Title)
                   .HasColumnName("title")
                   .IsRequired()
                    .HasMaxLength(500);

            builder.Property(u => u.Status)
                   .HasColumnName("status")
                   .IsRequired();

            builder.Property(u => u.ThumbnailUrl)
                   .HasColumnName("thumbnail_url")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.ImageList)
                   .HasColumnName("image_list")
                   .HasColumnType("text"); // JSON array stored as text

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);
            

            // Relationships 1-n
            builder.HasMany(u => u.Contents)
                   .WithOne(a => a.Blog)
                   .HasForeignKey(a => a.BlogId);

            builder.HasOne(u => u.Category)
                .WithMany(u => u.Blogs)
                .HasForeignKey(u => u.CategoryId); 
        }
    }
}