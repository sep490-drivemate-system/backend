using Microsoft.EntityFrameworkCore;
using MessagingService.Domain.Entities;

namespace MessagingService.Infrastructure.Persistence.Context
{
    public class MessagingDbContext : DbContext
    {
        public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessagingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}

