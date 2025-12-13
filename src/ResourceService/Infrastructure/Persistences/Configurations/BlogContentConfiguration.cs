using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
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
                   .HasColumnType("text");

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            // Relationship: BlogContent belongs to Blog
            builder.HasOne(u => u.Blog)
                   .WithMany(b => b.Contents)
                   .HasForeignKey(u => u.BlogId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
