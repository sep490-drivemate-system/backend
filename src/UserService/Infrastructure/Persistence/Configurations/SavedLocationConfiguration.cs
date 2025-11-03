using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class SavedLocationConfiguration : IEntityTypeConfiguration<SavedLocation>
    {
        public void Configure(EntityTypeBuilder<SavedLocation> builder)
        {
            // table name
            builder.ToTable("Address");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            // properties
            builder.Property(x => x.DisplayName)
                .HasColumnName("location_string")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.LocationLatitude)
                .HasColumnName("latitude")
                .IsRequired();

            builder.Property(x => x.LocationLongtitude)
                .HasColumnName("longtitude")
                .IsRequired();

            builder.Property(x => x.NoviceDriverId)
                .HasColumnName("novice_driver_id")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .HasColumnType("timestamp")
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("now()")
               .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}