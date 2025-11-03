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

            // foreign keys
            builder.Property(x => x.ApplicationId)
                .HasColumnName("application_id")
                .IsRequired();

            // properties
            builder.Property(x => x.Note)
                .HasColumnName("note")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("application_status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .HasColumnType("timestamp")
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("now()")
               .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.ApplicationId)
                   .HasColumnName("application_id")
                   .IsRequired();

            // relationships
            builder.HasOne(x => x.InstructorApplication)
                .WithMany(x => x.ApplicationTrackings)
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
