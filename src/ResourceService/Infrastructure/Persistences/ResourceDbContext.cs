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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
