using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Persistence.Configurations
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            // table name
            builder.ToTable("Refunds");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(r => r.PaymentId)
                   .HasColumnName("payment_id")
                   .IsRequired();

            builder.Property(r => r.RefundAmount)
                   .HasColumnName("refund_amount")
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(r => r.Reason)
                   .HasColumnName("reason")
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(r => r.Status)
                   .HasColumnName("status")
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(r => r.RefundTransactionId)
                   .HasColumnName("refund_transaction_id")
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(r => r.ProcessedAt)
                   .HasColumnName("processed_at")
                   .HasColumnType("timestamp")
                   .IsRequired(false);

            builder.Property(r => r.ProcessedBy)
                   .HasColumnName("processed_by")
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(r => r.CreatedAt)
                   .HasColumnName("created_at")
                   .HasColumnType("timestamp");

            builder.Property(r => r.IsDelete)
                   .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            // Relationships
            builder.HasOne(r => r.Payment)
                   .WithMany()
                   .HasForeignKey(r => r.PaymentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
