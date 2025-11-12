using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Repositories.Models;

namespace ResourceService.Repositories.Configurations
{
    public class BlogContentConfiguration : IEntityTypeConfiguration<BlogContent>
    {
        public void Configure(EntityTypeBuilder<BlogContent> builder)
        {
            // table name
            builder.ToTable("BlogContent");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.BlogId)
                   .HasColumnName("blog_id")
                   .IsRequired();


            builder.Property(u => u.Content)
                   .HasColumnName("content")
                   .IsRequired()
                    .HasMaxLength(500);

            builder.Property(u => u.No)
                   .HasColumnName("no")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.ImageUrl)
                   .HasColumnName("image_url")
                   .IsRequired()
                   .HasMaxLength(500);


            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);


        }
    }
}
