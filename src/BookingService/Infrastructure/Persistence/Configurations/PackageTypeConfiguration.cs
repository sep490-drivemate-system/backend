using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class PackageTypeConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            builder.Property(x => x.RecommendedValue)
                .HasColumnName("value")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("upadated_at")
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            // Relationships

            builder.HasMany(x => x.CarPackages)
                .WithOne(x => x.Packages)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Package");
        }
    }
}
