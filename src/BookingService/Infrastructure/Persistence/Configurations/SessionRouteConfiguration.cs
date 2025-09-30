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
                .IsRequired();

            builder.Property(x => x.RouteId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Session)
                .WithMany(x => x.SessionRoutes)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.RouteLog)
                .WithMany(x => x.SessionRoutes)
                .HasForeignKey(x => x.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("SessionRoutes");
        }
    }
}
