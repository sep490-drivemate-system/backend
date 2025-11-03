using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Domain.Enum;

namespace UserService.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // table name
            builder.ToTable("Users");

            // primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            // properties
            builder.Property(x => x.Username)
                .HasColumnName("user_name")
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.HashedPassword)
                .HasColumnName("hashed_password")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(11);

            builder.Property(x => x.Avatar)
                .HasColumnName("avatar")
                .HasMaxLength(500);

            builder.Property(x => x.DateOfBirth)
                .HasColumnName("date_of_birth")
                .IsRequired();

            builder.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Role)
                .HasColumnName("role")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.MaxLicenseLevel)
                .HasColumnName("driving_license_tier")
                .HasConversion<int>()
                .IsRequired(false);

            builder.Property(x => x.AccountStatus)
                .HasColumnName("account_status")
                .HasConversion<int>()
                .HasDefaultValue(AccountStatus.Normal)
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

            // Relationships 1-1
            builder.HasOne(x => x.NoviceDriver)
                .WithOne(x => x.User)
                .HasForeignKey<NoviceDriver>(x => x.Id);

            builder.HasOne(x => x.Instructor)
                .WithOne(i => i.User)
                .HasForeignKey<Instructor>(x => x.Id);
        }
    }
}
