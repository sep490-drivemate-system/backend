using Microsoft.EntityFrameworkCore;

namespace ResourceService.Infrastructure.Persistence.Context
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options)
        {
        }

        // Add DbSets for your entities here
        // Example: public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Apply configurations here
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceDbContext).Assembly);
        }
    }
}
