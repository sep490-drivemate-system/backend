using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class DrivingSessionConfiguration : IEntityTypeConfiguration<DrivingSession>
    {
        public void Configure(EntityTypeBuilder<DrivingSession> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.BookingId)
                .HasColumnName("booking_id")
                .IsRequired();

            builder.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            builder.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .IsRequired();

            builder.Property(x => x.ActualStart)
                .HasColumnName("actual_start_time");

            builder.Property(x => x.ActualEnd)
                .HasColumnName("actual_end_time");

            builder.Property(x => x.TotalDistance)
                .HasColumnName("distance");

            builder.Property(x => x.AverageSpeed)
                .HasColumnName("speed");

            builder.Property(x => x.StartingLatitude)
                .HasColumnName("starting_lat");

            builder.Property(x => x.StartingLongtitude)
                .HasColumnName("starting_long");

            builder.Property(x => x.EndingLatitude)
                .HasColumnName("end_lat");

            builder.Property(x => x.EndingLongtitude)
                .HasColumnName("end_long");

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Bookings)
                .WithMany(x => x.DrivingSessions)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SessionRoadTypes)
                .WithOne(x => x.DrivingSessions)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SessionLogs)
                .WithOne(x => x.DrivingSessions)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SessionRoutes)
                .WithOne(x => x.DrivingSessions)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("DrivingSession");
        }
    }
}
