using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            // table name
            builder.ToTable("DocumentType");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.Description)
                   .HasColumnName("description")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            // Relationships 1-n
            builder.HasMany(u => u.ApplicationTrackings)
                   .WithOne(a => a.DocumentType)
                   .HasForeignKey(a => a.TypeId);
        }
    }
}
