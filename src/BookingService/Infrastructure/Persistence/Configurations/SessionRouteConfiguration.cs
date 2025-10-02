using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class SessionRouteConfiguration : IEntityTypeConfiguration<SessionRoute>
    {
        public void Configure(EntityTypeBuilder<SessionRoute> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.SessionId)
                .HasColumnName("session_id")
                .IsRequired();

            builder.Property(x => x.TextInstruction)
                .HasColumnName("text_instruction")
                .IsRequired();

            builder.Property(x => x.StreetName)
                .HasColumnName("street_name")
                .IsRequired();

            builder.Property(x => x.LatitudeStart)
                .HasColumnName("latitude_start")
                .IsRequired();

            builder.Property(x => x.LongitudeStart)
                .HasColumnName("longtitude_start")
                .IsRequired();

            //builder.Property(x => x.LatitudeEnd)
            //    .HasColumnName("latitude_end")
            //    .IsRequired();

            //builder.Property(x => x.LongtitudeEnd)
            //    .HasColumnName("longtitude_end")
            //    .IsRequired();

            builder.Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.DrivingSessions)
                .WithMany(x => x.SessionRoutes)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("SessionRoute");
        }
    }
}
