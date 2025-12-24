using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookingService.Infrastructure.Persistence.Context
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        // Car related entities
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }

        // Package related entities
        public DbSet<DrivingSkill> DrivingSkills { get; set; }
        public DbSet<RoadType> RoadTypes { get; set; }
        public DbSet<Package> Packages { get; set; }
        
        // Booking related entities
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        // Session related entities
        public DbSet<DrivingSession> DrivingSessions { get; set; }
        public DbSet<InstructorRoutes> InstructorRoutes { get; set; }
        public DbSet<SessionRoute> SessionRoutes { get; set; }
        public DbSet<SessionLog> RouteLogs { get; set; }
        public DbSet<RescheduleRequest> RescheduleRequests { get; set; }
        
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
