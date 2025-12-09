using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences.Configurations
{
    public class VoucherUsageConfiguration : IEntityTypeConfiguration<VoucherUsage>
    {
        public void Configure(EntityTypeBuilder<VoucherUsage> builder)
        {
            builder.ToTable("VoucherUsage");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.VoucherId)
                .HasColumnName("voucher_id")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.PackageId)
                .HasColumnName("package_id")
                .IsRequired();

            builder.Property(x => x.DiscountAmount)
                .HasColumnName("discount_amount")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.OrderAmount)
                .HasColumnName("order_amount")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            // Relationships
            builder.HasOne(x => x.Voucher)
                .WithMany(v => v.VoucherUsages)
                .HasForeignKey(x => x.VoucherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

