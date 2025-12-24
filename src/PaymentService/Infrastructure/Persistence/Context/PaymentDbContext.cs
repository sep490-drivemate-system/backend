using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Infrastructure.Persistence.Configurations;

namespace PaymentService.Infrastructure.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
            //ConfigureLazyLoading(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
