using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class LicenseCategoryConfiguration : IEntityTypeConfiguration<LicenseCategory>
    {
        public void Configure(EntityTypeBuilder<LicenseCategory> builder)
        {
            // table name
            builder.ToTable("LicenseCategory");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(lc => lc.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(lc => lc.Priority)
                   .HasColumnName("priority")
                   .IsRequired();

            builder.Property(lc => lc.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(lc => lc.IsDeleted)
                   .HasColumnName("is_deleted")
                   .HasDefaultValue(false);

            builder.Property(lc => lc.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamp");

            // relationships
            builder.HasMany(lc => lc.Cars)
                   .WithOne(c => c.LicenseCategory)
                   .HasForeignKey(c => c.LicenseCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
