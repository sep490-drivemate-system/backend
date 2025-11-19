using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class InstructorApplicationConfiguration : IEntityTypeConfiguration<InstructorApplication>
    {
        public void Configure(EntityTypeBuilder<InstructorApplication> builder)
        {
            // table name
            builder.ToTable("InstructorApplication");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // foreign key
            builder.Property(x => x.DrivingLicenseTier)
                .HasColumnName("driving_license_level")
                .IsRequired();

            // Properties
            builder.Property(x => x.Fullname)
                .HasColumnName("fullname")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.EmailAddress)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.DateOfBirth)
                .HasColumnName("date_of_birth")
                .IsRequired();

            builder.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.HealthCheckup)
                .HasColumnName("health_checkup")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.BackgroundProfile)
                .HasColumnName("background_profile")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.DrivingLicenseFront)
                .HasColumnName("driving_license_front")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.DrivingLicenseBack)
                .HasColumnName("driving_license_back")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.DrivingLicenseTier)
                .HasColumnName("driving_license_tier")
                .IsRequired();

            builder.Property(x => x.TeachingLicenseFront)
                .HasColumnName("teaching_license_front")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.TeachingLicenseTier)
                .HasColumnName("teaching_license_tier")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(x => x.SubmitAt)
                .HasColumnName("submit_at")
                .HasColumnType("timestamp");

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
            builder.HasOne(x => x.Instructors)
                .WithOne(x => x.InstructorApplication)
                .HasForeignKey<InstructorApplication>(x => x.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ApplicationTrackings)
                .WithOne(x => x.InstructorApplication)
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}