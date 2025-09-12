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
            builder.Property(u => u.Note)
                   .HasColumnName("note")
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.Status)
                   .HasColumnName("status")
                   .IsRequired();

            builder.Property(u => u.ReviewerId)
                   .HasColumnName("reviewer_id")
                   .IsRequired();

            builder.Property(u => u.SubmitAt)
                   .HasColumnName("submit_at")
                   .HasColumnType("timestamp")
                   .IsRequired();

            builder.Property(u => u.ReviewAt)
                  .HasColumnName("review_at")
                  .HasColumnType("timestamp")
                   .IsRequired(); 

            builder.Property(u => u.UpdateAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamp");

            builder.Property(u => u.CreatedAt)
                  .HasColumnName("create_at")
                  .HasColumnType("timestamp");

            builder.Property(u => u.IsDelete)
                .HasColumnName("is_delete")
                   .HasDefaultValue(false);

            // Relationships 1-1
            builder.HasOne(u => u.Instructor)
                   .WithOne(nd => nd.InstructorApplication)
                   .HasForeignKey<InstructorApplication>(u => u.Id);

            // Relationships 1-n
            builder.HasOne(u => u.User)
                   .WithMany(user => user.InstructorApplications)
                   .HasForeignKey(u => u.ReviewerId);

            builder.HasMany(u => u.InstructorDocuments)
                   .WithOne(a => a.InstructorApplication)
                   .HasForeignKey(a => a.ApplicationId);
        }
    }
}