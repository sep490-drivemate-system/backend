using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class PackageCarConfiguration : IEntityTypeConfiguration<PackageCar>
    {
        public void Configure(EntityTypeBuilder<PackageCar> builder)
        {
            // table name
            builder.ToTable("PackageCar");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(pc => pc.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(pc => pc.IsDeleted)
                   .HasColumnName("is_deleted")
                   .HasDefaultValue(false);

            builder.Property(pc => pc.Price)
                   .HasColumnName("price")
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(pc => pc.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

            // foreign keys
            builder.Property(pc => pc.PackageId)
                   .HasColumnName("package_id")
                   .IsRequired();

            builder.Property(pc => pc.CarId)
                   .HasColumnName("car_id")
                   .IsRequired();

            // relationships
            builder.HasOne(pc => pc.Package)
                   .WithMany(p => p.PackageCars)
                   .HasForeignKey(pc => pc.PackageId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pc => pc.Car)
                   .WithMany()
                   .HasForeignKey(pc => pc.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            // indexes
            builder.HasIndex(pc => pc.PackageId);
            builder.HasIndex(pc => pc.CarId);

            // composite unique constraint
            builder.HasIndex(pc => new { pc.PackageId, pc.CarId })
                   .IsUnique();
        }
    }
}
