using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class CarImageConfiguration : IEntityTypeConfiguration<CarImage>
    {
        public void Configure(EntityTypeBuilder<CarImage> builder)
        {
            // table name
            builder.ToTable("CarImage");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(ci => ci.ImageUrl)
                   .HasColumnName("image_url")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(ci => ci.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(ci => ci.IsDeleted)
                   .HasColumnName("is_deleted")
                   .HasDefaultValue(false);

            builder.Property(ci => ci.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

            // foreign keys
            builder.Property(ci => ci.CarId)
                   .HasColumnName("car_id")
                   .IsRequired();

            // relationships
            builder.HasOne(ci => ci.Car)
                   .WithMany(c => c.CarImages)
                   .HasForeignKey(ci => ci.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            // indexes
            builder.HasIndex(ci => ci.CarId);
        }
    }
}
