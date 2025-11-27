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
            //ConfigureLazyLoading(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
