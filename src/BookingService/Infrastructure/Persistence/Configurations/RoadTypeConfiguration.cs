using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class RoadTypeConfiguration : IEntityTypeConfiguration<RoadType>
    {
        public void Configure(EntityTypeBuilder<RoadType> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Relationships configuration

            builder.HasMany(x => x.SessionRoadTypes)
                .WithOne(x => x.RoadTypes)
                .HasForeignKey(x => x.RoadTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("RoadType");
        }
    }
}
