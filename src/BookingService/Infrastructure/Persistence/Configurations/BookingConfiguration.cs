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

            builder.Property(x => x.DurationWhenBought)
                .HasColumnName("duration")
                .IsRequired();

            builder.Property(x => x.PriceAtBuyingTime)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.DriverId)
                .HasColumnName("driver_id")
                .IsRequired();

            builder.Property(x => x.InstructorId)
                .HasColumnName("instructor_id")
                .IsRequired();

            builder.Property(x => x.CarId)
                .HasColumnName("car_id")
                .IsRequired(false);

            builder.Property(x => x.PackageId)
                .HasColumnName("package_id")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
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

            // Relationships
            builder.HasOne(x => x.Car)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.CarId)
                .HasPrincipalKey(x => x.Id);

            builder.HasMany(x => x.DrivingSessions)
                .WithOne(x => x.Booking)
                .HasForeignKey(x => x.BookingId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Feedback)
                .WithOne(x => x.Booking)
                .HasForeignKey<Feedback>(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Package)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Booking");
        }
    }
}
