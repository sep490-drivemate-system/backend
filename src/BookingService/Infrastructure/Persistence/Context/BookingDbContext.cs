using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;

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
        public DbSet<SessionRoute> SessionRoutes { get; set; }
        public DbSet<SessionLog> RouteLogs { get; set; }
        public DbSet<RescheduleRequest> RescheduleRequests { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
