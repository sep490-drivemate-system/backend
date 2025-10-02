using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class CarPackageTypeConfiguration : IEntityTypeConfiguration<CarPackage>
    {
        public void Configure(EntityTypeBuilder<CarPackage> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CarId)
                .HasColumnName("car_id")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Relationships configurations
            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.CarPackages)
                .HasForeignKey(x => x.CarPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Packages)
                .WithMany(x => x.CarPackages)
                .HasForeignKey(x => x.PackageId);


            builder.ToTable("PackageType");
        }
    }
}
