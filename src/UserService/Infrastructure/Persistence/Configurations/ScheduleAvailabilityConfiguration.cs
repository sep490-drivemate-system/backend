using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class ScheduleAvailabilityConfiguration : IEntityTypeConfiguration<ScheduleAvailability>
    {
        public void Configure(EntityTypeBuilder<ScheduleAvailability> builder)
        {
            // table name
            builder.ToTable("ScheduleAvailability");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.StartTime)
                   .HasColumnName("start_time")
                   .IsRequired();

            builder.Property(u => u.InstructorId)
                   .HasColumnName("instructor_id")
                   .IsRequired(); 

            builder.Property(u => u.EndTime)
                   .HasColumnName("end_time")
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
