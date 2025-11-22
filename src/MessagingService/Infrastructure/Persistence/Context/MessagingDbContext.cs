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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessagingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}

