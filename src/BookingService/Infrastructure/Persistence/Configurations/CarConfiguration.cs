using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            // table name
            builder.ToTable("Car");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // foreign keys
            builder.Property(c => c.LicenseCategoryId)
                .HasColumnName("license_category_id")
                .IsRequired();

            builder.Property(c => c.ManufacturerId)
                .HasColumnName("manufacturer_id")
                .IsRequired();

            builder.Property(c => c.InstructorId)
                .HasColumnName("instructor_id")
                .IsRequired();

            // properties
            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2048)
                .IsRequired();

            builder.Property(c => c.ThumbnailUrl)
                .HasColumnName("thumbnail")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.DocumentJsonBlobString)
                .HasColumnName("document_blob")
                .HasMaxLength(2048)
                .IsRequired();

            builder.Property(c => c.InsuranceEndTime)
                .HasColumnName("insurance_end_time")
                .HasColumnType("date");

            builder.Property(c => c.FuelType)
                .HasColumnName("fuel")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.SeatCount)
                .HasColumnName("seat")
                .IsRequired();

            builder.Property(c => c.CarType)
                .HasColumnName("car_type")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Status)
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

            //builder.Property(c => c.InsuranceFrontUrl)
            //    .HasColumnName("insurance_front")
            //    .HasMaxLength(255)
            //    .IsRequired();

            //builder.Property(c => c.InsuranceBackUrl)
            //    .HasColumnName("insurance_back")
            //    .HasMaxLength(250)
            //    .IsRequired();

            //builder.Property(c => c.VehicleRegistrationFrontUrl)
            //    .HasColumnName("registration_front")
            //    .HasMaxLength(255)
            //    .IsRequired();

            //builder.Property(c => c.VehicleRegistrationBackUrl)
            //    .HasColumnName("registration_back")
            //    .HasMaxLength(250)
            //    .IsRequired();

            // relationships
            builder.HasOne(c => c.Manufacturer)
                .WithMany(m => m.Cars)
                .HasForeignKey(c => c.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CarImages)
                .WithOne(x => x.Car)
                .HasForeignKey(ci => ci.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Feedbacks)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
