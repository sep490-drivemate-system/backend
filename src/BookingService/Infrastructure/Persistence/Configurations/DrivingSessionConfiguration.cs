using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class DrivingSessionConfiguration : IEntityTypeConfiguration<DrivingSession>
    {
        public void Configure(EntityTypeBuilder<DrivingSession> builder)
        {
            builder.ToTable("DrivingSession");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // foreign keys 
            builder.Property(x => x.BookingId)
                .HasColumnName("booking_id")
                .IsRequired();

            // properties
            builder.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(x => x.PriceForCar)
              .HasColumnName("price_for_car")
              .IsRequired(false);

            builder.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(x => x.ActualStart)
                .HasColumnName("actual_start_time")
                .HasColumnType("timestamptz");

            builder.Property(x => x.ActualEnd)
                .HasColumnName("actual_end_time")
                .HasColumnType("timestamptz");

            builder.Property(x => x.TotalDistance)
                .HasColumnName("distance");

            builder.Property(x => x.AverageSpeed)
                .HasColumnName("speed");

            builder.Property(x => x.DisplayName)
               .HasColumnName("display_name");

            builder.Property(x => x.StartingLatitude)
                .HasColumnName("starting_lat");

            builder.Property(x => x.StartingLongtitude)
                .HasColumnName("starting_long");

            builder.Property(x => x.EndingLatitude)
                .HasColumnName("end_lat");

            builder.Property(x => x.EndingLongtitude)
                .HasColumnName("end_long");

            builder.Property(x => x.NoviceDriverNote)
                .HasColumnName("novice_driver_note")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.InstructorNote)
                .HasColumnName("instructor_note")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Booking)
                .WithMany(x => x.DrivingSessions)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SessionRoutes)
                .WithOne(x => x.DrivingSessions)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SessionLogs)
                .WithOne(x => x.DrivingSession)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
