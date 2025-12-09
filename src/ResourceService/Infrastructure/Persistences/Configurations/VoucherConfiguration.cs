using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Voucher");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            builder.Property(x => x.DiscountPercentage)
                .HasColumnName("discount_percentage")
                .IsRequired()
                .HasColumnType("decimal(5,2)"); // Ví dụ: 10.50 = 10.5%

            builder.Property(x => x.MinOrderAmount)
                .HasColumnName("min_order_amount")
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.MaxDiscountAmount)
                .HasColumnName("max_discount_amount")
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.UsageLimit)
                .HasColumnName("usage_limit");

            builder.Property(x => x.UsedCount)
                .HasColumnName("used_count")
                .HasDefaultValue(0);

            builder.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .IsRequired()
                .HasColumnType("timestamp");

            builder.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .IsRequired()
                .HasColumnType("timestamp");

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            // Index cho Code để tìm kiếm nhanh
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("\"is_deleted\" = false");

            // Relationships
            builder.HasMany(x => x.VoucherUsages)
                .WithOne(vu => vu.Voucher)
                .HasForeignKey(vu => vu.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

