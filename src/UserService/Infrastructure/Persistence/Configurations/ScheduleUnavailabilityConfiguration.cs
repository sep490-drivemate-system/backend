using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class ScheduleUnavailabilityConfiguration : IEntityTypeConfiguration<ScheduleUnavailability>
    {
        public void Configure(EntityTypeBuilder<ScheduleUnavailability> builder)
        {
            // table name
            builder.ToTable("ScheduleUnavailability");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(su => su.Date)
                   .HasColumnName("date")
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(su => su.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(su => su.IsDelete)
                   .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            builder.Property(su => su.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

            // foreign keys
            builder.Property(su => su.InstructorId)
                   .HasColumnName("instructor_id")
                   .IsRequired();

            // relationships
            // Relationship configured from Instructor side

            // indexes
            builder.HasIndex(su => su.InstructorId);
            builder.HasIndex(su => su.Date);

            // composite unique constraint to prevent duplicate unavailability for same instructor on same date
            builder.HasIndex(su => new { su.InstructorId, su.Date })
                   .IsUnique();
        }
    }
}
