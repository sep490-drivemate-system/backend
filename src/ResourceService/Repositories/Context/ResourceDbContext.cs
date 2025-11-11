using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Base;

namespace ResourceService.Repositories.Models
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext()
        {
        }

        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogContent> BlogContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceDbContext).Assembly);
            //ConfigureLazyLoading(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
