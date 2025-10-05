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

            // properties
            builder.Property(u => u.BackgroundProfile)
                   .HasColumnName("note")
                   .IsRequired()
                   .HasMaxLength(500);
            // Citizen ID
            builder.Property(u => u.CitizenIdNumber)
               .HasColumnName("citizen_id_number")
               .IsRequired();
            builder.Property(u => u.CitizenIdFront)
                   .HasColumnName("citizen_id_front")
                   .IsRequired();

            builder.Property(u => u.CitizenIdBack)
                   .HasColumnName("citizen_id_back")
                   .IsRequired();

            builder.Property(u => u.CitizenIssueDate)
                   .HasColumnName("citizen_issue_date")
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(u => u.CitizenExpiryDate)
                   .HasColumnName("citizen_expiry_date")
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(u => u.CitizenIssuePlace)
                   .HasColumnName("citizen_issue_place")
                   .IsRequired();

            builder.Property(u => u.PermanentAddress)
                   .HasColumnName("permanent_address")
                   .IsRequired();

            builder.Property(u => u.CitizenIdStatus)
                   .HasColumnName("citizen_id_status")
                   .IsRequired();

            // DrivingLicense
            builder.Property(u => u.DrivingLicenseFront)
                   .HasColumnName("driving_license_front")
                   .IsRequired();
            builder.Property(u => u.DrivingLicenseBack)
                   .HasColumnName("driving_license_back")
                   .IsRequired();
            builder.Property(u => u.DrivingLicenseNumber)
                    .HasColumnName("driving_license_number")
                    .IsRequired();
            builder.Property(u => u.DrivingLicenseIssueDate)
                   .HasColumnName("driving_license_issue_date")
                   .HasColumnType("date")
                   .IsRequired();
            builder.Property(u => u.DrivingLicenseExpiry)
                    .HasColumnName("driving_license_expiry")
                    .HasColumnType("date");
            builder.Property(u => u.DrivingLicenseStatus)
                  .HasColumnName("driving_license_status")
                  .IsRequired();

            // Teaching License
            builder.Property(u => u.TeachingLicenseFront)
                 .HasColumnName("teaching_license_front")
                 .IsRequired();

            builder.Property(u => u.TeachingLicenseBack)
                   .HasColumnName("teaching_license_back")
                   .IsRequired();

            builder.Property(u => u.TeachingStatus)
                   .HasColumnName("teaching_status")
                   .IsRequired();

            builder.Property(u => u.BackgroundProfileStatus)
                   .HasColumnName("background_profile_status")
                   .IsRequired();

            builder.Property(u => u.SubmitAt)
                   .HasColumnName("submit_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("date");

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            builder.Property(u => u.Status)
                   .HasColumnName("status")
                   .IsRequired();

            // foreign keys

            // Relationships 1-1
            builder.HasOne(u => u.Instructor)
                   .WithOne(i => i.InstructorApplication)
                   .HasForeignKey<InstructorApplication>(u => u.Id);

            // Relationships 1-n
            builder.HasMany(u => u.ApplicationTracking)
                 .WithOne(a => a.InstructorApplication)
                 .HasForeignKey(a => a.ApplicationId);
        }
    }
}