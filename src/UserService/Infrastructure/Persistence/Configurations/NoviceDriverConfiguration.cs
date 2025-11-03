using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class NoviceDriverConfiguration : IEntityTypeConfiguration<NoviceDriver>
    {
        public void Configure(EntityTypeBuilder<NoviceDriver> builder)
        {
            // table name
            builder.ToTable("NoviceDriver");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id");

            // foreign keys
            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            // properties
            builder.Property(x => x.DrivingLicense)
                .HasColumnName("driving_license_image_url")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.DrivingLicenseExpirationDate)
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

            builder.HasOne(x => x.User)
                .WithOne(x => x.NoviceDriver)
                .HasForeignKey<NoviceDriver>(x => x.UserId);
        }
    }
}
