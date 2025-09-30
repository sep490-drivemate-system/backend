using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class ApplicationTrackingConfiguration : IEntityTypeConfiguration<ApplicationTracking>
    {
        public void Configure(EntityTypeBuilder<ApplicationTracking> builder)
        {
            // table name
            builder.ToTable("ApplicationTracking");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.Note)
                   .HasColumnName("note")
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                   .HasColumnName("is_delete")
                   .HasDefaultValue(false);


            builder.Property(u => u.TypeId)
                   .HasColumnName("type_id")
                   .IsRequired();
            builder.Property(u => u.ApplicationId)
                   .HasColumnName("application_id")
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

            // foreign keys

            // relationships
        }
    }
}
