using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class RoadTypeConfiguration : IEntityTypeConfiguration<RoadType>
    {
        public void Configure(EntityTypeBuilder<RoadType> builder)
        {
            // table name
            builder.ToTable("RoadType");
            
            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // properties
            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
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

            // relationships
            builder.HasMany(x => x.Packages)
                .WithMany(x => x.RoadTypes);
        }
    }
}
