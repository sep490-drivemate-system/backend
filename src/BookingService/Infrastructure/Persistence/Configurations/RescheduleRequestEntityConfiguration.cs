using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class RescheduleRequestEntityConfiguration : IEntityTypeConfiguration<RescheduleRequest>
    {
        public void Configure(EntityTypeBuilder<RescheduleRequest> builder)
        {
            // table name
            builder.ToTable("RescheduleRequest");

            // primary keys
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // properties
            builder.Property(x => x.Note)
                .HasColumnName("note")
                .IsRequired();

            builder.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            builder.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .IsRequired();

            builder.Property(x => x.Side)
                .HasColumnName("request_side")
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
            builder.HasOne(x => x.DrivingSessions)
                .WithMany(x => x.RescheduleRequests)
                .HasForeignKey(x => x.SessionId);
        }
    }
}
