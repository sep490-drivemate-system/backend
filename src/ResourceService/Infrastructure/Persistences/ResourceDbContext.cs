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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
