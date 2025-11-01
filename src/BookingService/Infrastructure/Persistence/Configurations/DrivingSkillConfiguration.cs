using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence.Configurations
{
    public class DrivingSkillConfiguration : IEntityTypeConfiguration<DrivingSkill>
    {
        public void Configure(EntityTypeBuilder<DrivingSkill> builder)
        {
            builder.ToTable("DrivingSkill");

            // primary key
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);

            

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.IllustrationUrl)
                .HasColumnName("thumbnail")
                .HasMaxLength(255);

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

            // Relationships configuration
            builder.HasMany(x => x.Packages)
                .WithMany(x => x.DrivingSkills);
        }
    }
}