using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class ScheduleAvailabilityConfiguration : IEntityTypeConfiguration<ScheduleUnavailability>
    {
        public void Configure(EntityTypeBuilder<ScheduleUnavailability> builder)
        {
            // table name
            builder.ToTable("ScheduleUnAvailability");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.Date)
                   .HasColumnName("date")
                   .IsRequired();

            builder.Property(u => u.InstructorId)
                   .HasColumnName("instructor_id")
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
