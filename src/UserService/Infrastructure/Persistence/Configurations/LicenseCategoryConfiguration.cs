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
            builder.Property(x => x.Id)
                .HasColumnName("id");

            // properties
            builder.Property(lc => lc.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(lc => lc.Priority)
                .HasColumnName("priority")
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
            builder.HasMany(x => x.Users)
                   .WithOne(x => x.LicenseCategory)
                   .HasForeignKey(x => x.MaxLicenseLevel)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
