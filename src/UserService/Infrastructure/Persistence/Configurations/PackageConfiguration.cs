using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            // table name
            builder.ToTable("Package");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(p => p.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Description)
                   .HasColumnName("description")
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .HasColumnName("price")
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.DurationDays)
                   .HasColumnName("duration_days")
                   .IsRequired();

            builder.Property(p => p.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(p => p.IsDeleted)
                   .HasColumnName("is_deleted")
                   .HasDefaultValue(false);

            builder.Property(p => p.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

            // relationships
            builder.HasMany(p => p.PackageCars)
                   .WithOne(pc => pc.Package)
                   .HasForeignKey(pc => pc.PackageId)
                   .OnDelete(DeleteBehavior.Cascade);

            // indexes
            builder.HasIndex(p => p.Name);
        }
    }
}
