using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // table name
            builder.ToTable("RefreshToken");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.ExpiryTime)
                   .HasColumnName("expiry_time")
                   .IsRequired()
                   .HasColumnType("timestamp")
                   .HasMaxLength(500);

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.RefreshKey)
                  .HasColumnName("refresh_key")
                  .IsRequired();

            // Relationships 1-1
            builder.HasOne(u => u.User)
                   .WithOne(nd => nd.RefreshToken)
                   .HasForeignKey<User>(nd => nd.Id);
        }
    }
}