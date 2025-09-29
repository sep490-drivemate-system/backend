using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
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

            // properties
            builder.Property(c => c.Thumbnail)
                   .HasColumnName("thumbnail")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(c => c.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Description)
                   .HasColumnName("description")
                   .HasMaxLength(1000);

            builder.Property(c => c.Insurance)
                   .HasColumnName("insurance")
                   .IsRequired()
                   .HasMaxLength(100);


            builder.Property(c => c.InsuranceEndTime)
                   .HasColumnName("insurance_end_time")
                   .HasColumnType("date");

            builder.Property(c => c.VehicleRegistration)
                   .HasColumnName("vehicle_registration")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(c => c.Seat)
                   .HasColumnName("seat")
                   .IsRequired();

            builder.Property(c => c.CartType)
                   .HasColumnName("car_type")
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(c => c.IsDeleted)
                   .HasColumnName("is_deleted")
                   .HasDefaultValue(false);

            builder.Property(c => c.Status)
                   .HasColumnName("status")
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

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

            // relationships
            builder.HasOne(c => c.LicenseCategory)
                   .WithMany(lc => lc.Cars)
                   .HasForeignKey(c => c.LicenseCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(c => c.CarImages)
                   .WithOne(ci => ci.Car)
                   .HasForeignKey(ci => ci.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Manufacturer)
        .WithMany(m => m.Cars)
        .HasForeignKey(c => c.ManufacturerId)
        .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
