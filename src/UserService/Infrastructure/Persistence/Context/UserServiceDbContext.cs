using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Context
{
    public class UserServiceDbContext : DbContext
    {
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
        {
        }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }        
        public DbSet<SavedLocation> SavedLocations { get; set; }
        public DbSet<NoviceDriver> NoviceDrivers { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<PersonalSchedule> PersonalSchedules { get; set; }
        public DbSet<ApplicationTracking> InstructorApplications { get; set; }
        public DbSet<InstructorApplication> InstructorDocuments { get; set; }
        public DbSet<SystemDocument> SystemDocuments { get; set; }
        public DbSet<Policy> Policies { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Tự động cộng 7 giờ (UTC+7) cho các property DateTime khi tạo mới hoặc cập nhật
            var vietnamTimeOffset = TimeSpan.FromHours(7);
            
            var addedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            // Xử lý CreatedAt khi tạo mới
            foreach (var entry in addedEntries)
            {
                AdjustDateTimeProperty(entry.Entity, "CreatedAt", vietnamTimeOffset);
            }

            // Xử lý UpdatedAt/LastModifiedAt khi cập nhật
            foreach (var entry in modifiedEntries)
            {
                AdjustDateTimeProperty(entry.Entity, "UpdatedAt", vietnamTimeOffset);
                AdjustDateTimeProperty(entry.Entity, "LastModifiedAt", vietnamTimeOffset);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AdjustDateTimeProperty(object entity, string propertyName, TimeSpan offset)
        {
            var entityType = entity.GetType();
            var property = entityType.GetProperties()
                .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)
                    && (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?)));

            if (property != null)
            {
                var currentValue = property.GetValue(entity);
                
                if (property.PropertyType == typeof(DateTime) && currentValue != null)
                {
                    var dateTimeValue = (DateTime)currentValue;
                    property.SetValue(entity, dateTimeValue.Add(offset));
                }
                else if (property.PropertyType == typeof(DateTime?) && currentValue != null)
                {
                    var dateTimeValue = (DateTime?)currentValue;
                    if (dateTimeValue.HasValue)
                    {
                        property.SetValue(entity, dateTimeValue.Value.Add(offset));
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
