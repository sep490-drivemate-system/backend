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
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.AllowedBooking)
                   .HasColumnName("allowed_booking")
                   .IsRequired();

            builder.Property(u => u.DrivingLicenseImageUrl)
                   .HasColumnName("driving_license_image_url")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.UpdateAt)
                 .HasColumnName("update_at")
                 .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                  .HasColumnName("is_delete")
                  .HasDefaultValue(false)
                  .IsRequired();
        }
    }
}
