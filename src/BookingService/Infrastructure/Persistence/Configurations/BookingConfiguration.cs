using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.DriverId)
                .HasColumnName("driver_id")
                .IsRequired();

            builder.Property(x => x.CarPackageId)
                .HasColumnName("package_id")
                .IsRequired();

            builder.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .IsRequired();

            builder.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.DrivingSessions)
                .WithOne(x => x.Bookings)
                .HasForeignKey(x => x.BookingId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Feedbacks)
                .WithOne(x => x.Bookings)
                .HasForeignKey(x => x.BookingId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.CarPackages)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.CarPackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Booking");
        }
    }
}
