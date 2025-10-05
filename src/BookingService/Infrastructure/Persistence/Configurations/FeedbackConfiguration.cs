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

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
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
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Bookings)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Feedback");
        }
    }
}
