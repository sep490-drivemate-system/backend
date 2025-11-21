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
            builder.ToTable("SavedLocation");

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

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
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

            builder.HasOne(x => x.User)
                .WithMany(x => x.SavedLocations)
                .HasForeignKey(x => x.UserId);
        }
    }
}