using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class RouteLogConfiguration : IEntityTypeConfiguration<RouteLog>
    {
        public void Configure(EntityTypeBuilder<RouteLog> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.StartLocation)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.EndLocation)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Distance)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.EstimatedDuration)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.SessionRoutes)
                .WithOne(x => x.RouteLog)
                .HasForeignKey(x => x.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("RouteLogs");
        }
    }
}
