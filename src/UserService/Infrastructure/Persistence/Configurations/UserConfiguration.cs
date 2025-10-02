using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

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
            builder.Property(u => u.UserName)
                   .HasColumnName("user_name")
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(u => u.Email)
                   .HasColumnName("email")
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.HashedPassword)
                   .HasColumnName("hashed_password")
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.PhoneNumber)
                   .HasColumnName("phone_number")
                   .IsRequired(false)
                   .HasMaxLength(11);

            builder.Property(u => u.Avatar)
                  .HasColumnName("avatar")
                  .HasMaxLength(500)
                   .IsRequired(false);
            builder.Property(u => u.DateOfBirth)
                  .HasColumnName("date_of_birth")
                  .HasColumnType("date");

            builder.Property(u => u.Role)
                   .HasColumnName("role")
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(u => u.Gender)
                   .HasColumnName("gender")
                   .HasConversion<int>();

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
            builder.HasOne(u => u.NoviceDriver)
                   .WithOne(nd => nd.User)
                   .HasForeignKey<NoviceDriver>(nd => nd.Id);

            builder.HasOne(u => u.Instructor)
                   .WithOne(i => i.User)
                   .HasForeignKey<Instructor>(i => i.Id);

            builder.HasOne(u => u.RefreshToken)
                   .WithOne(rt => rt.User)
                   .HasForeignKey<RefreshToken>(rt => rt.Id);

            // Relationships 1-n
            builder.HasMany(u => u.Addresses)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserId);
        }
    }
}
