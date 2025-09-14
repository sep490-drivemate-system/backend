using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class PackageInstructorConfiguration : IEntityTypeConfiguration<PackageInstructor>
    {
        public void Configure(EntityTypeBuilder<PackageInstructor> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.PackageId)
                .IsRequired();

            builder.Property(x => x.InstructorId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Package)
                .WithMany(x => x.PackageInstructors)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("PackageInstructors");
        }
    }
}
