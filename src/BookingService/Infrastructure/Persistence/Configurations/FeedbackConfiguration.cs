using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.BookingId)
                .HasColumnName("booking_id")
                .IsRequired();

            builder.Property(x => x.CarId)
                .HasColumnName("car_id")
                .IsRequired();

            builder.Property(x => x.NoviceDriverId)
                .HasColumnName("novice_driver_id")
                .IsRequired();

            builder.Property(x => x.InstructorId)
                .HasColumnName("instructor_id")
                .IsRequired();

            builder.Property(x => x.InstructorRating)
                .HasColumnName("rating_instructor")
                .IsRequired();

            builder.Property(x => x.InstructorFeedback)
                .HasColumnName("description_instructor")
                .HasMaxLength(1000);

            builder.Property(x => x.CarRating)
                .HasColumnName("rating_car")
                .IsRequired();

            builder.Property(x => x.CarFeedback)
                .HasColumnName("description_car")
                .HasMaxLength(1000);

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

            // Relationships
            // The Booking - Feedback relationship is already been defined in the Booking entity config.

            builder.ToTable("Feedback");
        }
    }
}
