using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class InstructorRoutesConfiguration : IEntityTypeConfiguration<InstructorRoutes>
    {
        public void Configure(EntityTypeBuilder<InstructorRoutes> builder)
        {
            // table name
            builder.ToTable("InstructorRoutes");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // foreign keys
            builder.Property(x => x.CreatedAt)
                .HasColumnName("create_at")
                .HasColumnType("timestamp")
                .IsRequired();

            // properties
            builder.Property(x => x.RouteName)
               .HasColumnName("route_name")
               .HasMaxLength(1000)
               .IsRequired();

            builder.Property(x => x.Polyline)
            .HasColumnName("polyline")
             .HasColumnType("text")
             .IsRequired();

            builder.Property(x => x.StartingLatitude)
               .HasColumnName("starting_latitude")
               .HasColumnType("decimal(10,6)");

            builder.Property(x => x.StartingLongtitude)
                   .HasColumnName("starting_longitude")
                   .HasColumnType("decimal(10,6)");

            builder.Property(x => x.DisplayStartLocationName)
                   .HasColumnName("display_start_location_name")
                   .HasMaxLength(200);

            builder.Property(x => x.EndingLatitude)
                .HasColumnName("ending_latitude")
                .HasColumnType("decimal(10,6)");

            builder.Property(x => x.EndingLongtitude)
                   .HasColumnName("ending_longitude")
                   .HasColumnType("decimal(10,6)");

            builder.Property(x => x.DisplayEndLocationName)
                   .HasColumnName("display_end_location_name")
                   .HasMaxLength(200);


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

            builder.Property(x => x.InstructorId)
                   .HasColumnName("instructor_id")
                   .IsRequired();

            // relationships
            builder.HasMany(x => x.Packages)
                .WithMany(x => x.InstructorRoutes);
        }
    }
}
