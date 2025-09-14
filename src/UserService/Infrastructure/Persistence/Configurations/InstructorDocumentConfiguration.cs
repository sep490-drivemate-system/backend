using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class InstructorDocumentConfiguration : IEntityTypeConfiguration<InstructorDocument>
    {
        public void Configure(EntityTypeBuilder<InstructorDocument> builder)
        {
            // table name
            builder.ToTable("InstructorDocument");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.ApplicationId)
                   .HasColumnName("application_id")
                   .IsRequired();

            builder.Property(u => u.TypeId)
                   .HasColumnName("type_id")
                   .IsRequired();

            builder.Property(u => u.Note)
                   .HasColumnName("note")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.ImageURL)
       .HasColumnName("image_url")
       .IsRequired()
       .HasMaxLength(500);

            builder.Property(u => u.Status)
                   .HasColumnName("status")
                   .IsRequired();

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);

        }
    }
}
