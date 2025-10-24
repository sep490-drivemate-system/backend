using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            // table name
            builder.ToTable("Wallets");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(w => w.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.Property(w => w.Balance)
                   .HasColumnName("balance")
                   .HasPrecision(18, 2)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(w => w.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasColumnType("timestamp");

            builder.Property(w => w.UpdatedDate)
                   .HasColumnName("updated_date")
                   .HasColumnType("timestamp");

            builder.Property(w => w.CreatedAt)
                   .HasColumnName("created_at")
                   .HasColumnType("timestamp");

            builder.Property(w => w.IsDelete)
                   .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            // Relationships
            builder.HasMany(w => w.Transactions)
                   .WithOne(t => t.Wallet)
                   .HasForeignKey(t => t.FromWalletId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
