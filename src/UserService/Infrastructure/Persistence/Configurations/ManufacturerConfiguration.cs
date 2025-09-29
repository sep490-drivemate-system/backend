using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
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
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(b => b.IsDeleted)
                   .HasColumnName("is_deleted")
                   .HasDefaultValue(false);

            builder.Property(b => b.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

            // relationships
            builder.HasMany(b => b.Cars)
                   .WithOne(c => c.Manufacturer)
                   .HasForeignKey(c => c.ManufacturerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
