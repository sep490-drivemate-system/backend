using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("Policy");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(u => u.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.Description)
                  .HasColumnName("description")
                  .IsRequired();

            builder.Property(u => u.PolicyType)
                  .HasColumnName("type")
                  .IsRequired();

            builder.Property(u => u.UpdatedAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.IsDeleted)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);
        }
    }
}
