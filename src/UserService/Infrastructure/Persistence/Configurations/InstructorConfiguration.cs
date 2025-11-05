using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            // table name
            builder.ToTable("Instructor");

            // primary keys
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            // foreign keys
            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            // properties
            builder.Property(x => x.Bio)
                .HasColumnName("bio")
                .HasMaxLength(1024)
                .IsRequired(false);

            builder.Property(x => x.Experience)
                .HasColumnName("experience")
                .IsRequired();
                  
            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<int>()
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

            // Relationships
            builder.HasOne(x => x.User)
                   .WithOne(x => x.Instructor)
                   .HasForeignKey<Instructor>(x => x.Id);

            builder.HasMany(x => x.InstructorSchedules)
                   .WithOne(x => x.Instructor)
                   .HasForeignKey(x => x.InstructorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.InstructorApplication)
                   .WithOne(x => x.Instructors)
                   .HasForeignKey<InstructorApplication>(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
