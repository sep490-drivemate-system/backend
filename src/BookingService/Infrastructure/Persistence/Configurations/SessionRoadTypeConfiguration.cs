using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class SessionRoadTypeConfiguration : IEntityTypeConfiguration<SessionRoadType>
    {
        public void Configure(EntityTypeBuilder<SessionRoadType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.SessionId)
                .HasColumnName("session_id")
                .IsRequired();

            builder.Property(x => x.RoadTypeId)
                .HasColumnName("road_type_id")
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

            builder.HasOne(x => x.RoadTypes)
                .WithMany(x => x.SessionRoadTypes)
                .HasForeignKey(x => x.RoadTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.DrivingSessions)
                .WithMany(x => x.SessionRoadTypes)
                .HasForeignKey(x => x.SessionId);

            builder.ToTable("SessionRoadType");
        }
    }
}
