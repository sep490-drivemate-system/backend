using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations
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
            
            // foreign keys
            builder.Property(x => x.CarId)
                .HasColumnName("car_id")
                .IsRequired();

            // properties
            builder.Property(x => x.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500)
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

            // relationships
            builder.HasOne(x => x.Car)
                   .WithMany(c => c.CarImages)
                   .HasForeignKey(x => x.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            // indexes
            builder.HasIndex(x => x.CarId);
        }
    }
}
