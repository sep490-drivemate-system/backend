using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // table name
            builder.ToTable("Address");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.Location)
                   .HasColumnName("location")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.UserId)
                  .HasColumnName("user_id")
                  .IsRequired();

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);
        }
    }
}