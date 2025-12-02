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

            builder.Property(w => w.Balance)
                   .HasColumnName("balance")
                   .HasPrecision(18, 2)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(x => x.CreatedAt)
     .HasColumnName("created_at")
     .HasColumnType("timestamptz")
     .ValueGeneratedOnAdd()
     .HasDefaultValueSql("now()")
     .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(w => w.IsDelete)
                   .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            // Relationships
            builder.HasMany(w => w.Transactions)
                   .WithOne(t => t.Wallet)
                   .HasForeignKey(t => t.FromWalletId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
