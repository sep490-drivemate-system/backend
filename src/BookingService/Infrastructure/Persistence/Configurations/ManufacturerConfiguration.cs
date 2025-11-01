using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            // table name
            builder.ToTable("Manufacturer");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(b => b.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
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
            builder.HasMany(b => b.Cars)
                   .WithOne(c => c.Manufacturer)
                   .HasForeignKey(c => c.ManufacturerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
