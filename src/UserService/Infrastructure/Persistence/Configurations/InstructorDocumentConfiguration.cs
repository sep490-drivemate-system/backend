using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class InstructorDocumentConfiguration : IEntityTypeConfiguration<InstructorApplication>
    {
        public void Configure(EntityTypeBuilder<InstructorApplication> builder)
        {
            // table name
            builder.ToTable("InstructorDocument");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.CitizenIdBack)
                   .HasColumnName("citizen_id_back")
                   .IsRequired();

            builder.Property(u => u.CitizenIdFront)
                   .HasColumnName("citizen_id_front")
                   .IsRequired();

            builder.Property(u => u.PermanentAddress)
                   .HasColumnName("permanent_address")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.CitizenExpiryDate)
                   .HasColumnName("citizen_expiry_date")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.Status)
                   .HasColumnName("status")
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
