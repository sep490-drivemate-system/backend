using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            // table name
            builder.ToTable("Transactions");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(t => t.BookingId)
                   .HasColumnName("booking_id")
                   .IsRequired();

            builder.Property(t => t.TransactionValue)
                   .HasColumnName("transaction_value")
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(t => t.PaymentMethod)
                   .HasColumnName("payment_method")
                   .HasConversion<int>()
                   .IsRequired(false);

            builder.Property(t => t.Status)
                   .HasColumnName("status")
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(t => t.ReferenceCode)
                   .HasColumnName("reference_code")
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(t => t.ToWalletId)
                   .HasColumnName("to_wallet_id")
                   .IsRequired(false);

            builder.Property(t => t.FromWalletId)
                   .HasColumnName("from_wallet_id")
                   .IsRequired();



            builder.Property(t => t.IsDelete)
                   .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            // Relationships
            builder.HasOne(t => t.Wallet)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.FromWalletId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
