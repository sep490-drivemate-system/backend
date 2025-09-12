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

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(u => u.Bio)
                   .HasColumnName("bio")
                   .IsRequired(false)
                   .HasMaxLength(1000);

            builder.Property(u => u.Experience)
                   .HasColumnName("experience")
                   .IsRequired();
                   

            builder.Property(u => u.Status)
                   .HasColumnName("status")
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
            builder.HasOne(u => u.User)
                   .WithOne(nd => nd.Instructor)
                   .HasForeignKey<Instructor>(nd => nd.Id);

            // Relationships 1-n
            builder.HasMany(u => u.ScheduleAvailabilities)
                   .WithOne(a => a.Instructor)
                   .HasForeignKey(a => a.InstructorId);
        }
    }
}
