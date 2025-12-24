using Microsoft.EntityFrameworkCore;
using ResourceService.Domain.Entities;

namespace ResourceService.Infrastructure.Persistences
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext() { }

        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options) { }

        // Blog entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogContent> BlogContents { get; set; }
        
        // Quiz entities
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Answer> Answers { get; set; }
        
        // Voucher entities
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherUsage> VoucherUsages { get; set; }

        // Post / Forum entities
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostVideo> PostVideos { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<PostReview> PostReviews { get; set; }
        // QA entities
        public DbSet<QaQuestion> QaQuestions { get; set; }
        public DbSet<QaAnswer> QaAnswers { get; set; }

        // Tags
        public DbSet<Tag> Tags { get; set; }

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
