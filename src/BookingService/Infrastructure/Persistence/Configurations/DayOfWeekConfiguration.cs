using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class DayOfWeekConfiguration : IEntityTypeConfiguration<Domain.Entities.DayOfWeek>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.DayOfWeek> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.DayNumber)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.ToTable("DaysOfWeek");
        }
    }
}
