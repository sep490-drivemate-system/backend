using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class SessionLogConfiguration : IEntityTypeConfiguration<SessionLog>
    {
        public void Configure(EntityTypeBuilder<SessionLog> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.SessionId)
                .HasColumnName("session_id")
                .IsRequired();

            builder.Property(x => x.StreetName)
                .HasColumnName("street_name")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Speed)
                .HasColumnName("speed")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Heading)
                .HasColumnName("heading")
                .IsRequired();

            builder.Property(x => x.Latitude)
                .HasColumnName("latitude")
                .IsRequired();

            builder.Property(x => x.Longitude)
                .HasColumnName("longtitude")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            // Relationships configuration
            builder.HasOne(x => x.DrivingSessions)
                .WithMany(x => x.SessionLogs)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("RouteLog");
        }
    }
}
