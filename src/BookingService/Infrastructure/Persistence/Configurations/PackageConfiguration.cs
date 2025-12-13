using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            // table name
            builder.ToTable("Package");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // foreign key
            builder.Property(x => x.InstructorId)
               .HasColumnName("instructor_id")
               .IsRequired();

            // properties
            builder.Property(x => x.Name)
               .HasColumnName("name")
               .IsRequired();

            builder.Property(x => x.Description)
               .HasColumnName("description")
               .IsRequired();

            builder.Property(x => x.ThumbnailUrl)
               .HasColumnName("thumbnail")
               .IsRequired();

            builder.Property(x => x.Duration)
               .HasColumnName("duration")
               .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.IsRentalCar)
                .HasColumnName("is_rental_car")
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
            builder.HasMany(x => x.RoadTypes)
               .WithMany(x => x.Packages);

            builder.HasMany(x => x.DrivingSkills)
                .WithMany(x => x.Packages);

            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.Package)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Cars)
                .WithMany(x => x.Packages);

            
        }
    }
}
