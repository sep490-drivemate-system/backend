using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;

namespace BookingService.Infrastructure.Persistence.Context
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageInstructor> PackageInstructors { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<DrivingSession> DrivingSessions { get; set; }
        public DbSet<SessionRoute> SessionRoutes { get; set; }
        public DbSet<RouteLog> RouteLogs { get; set; }
        public DbSet<RoadType> RoadTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<BookingTimeRange> BookingTimeRanges { get; set; }
        public DbSet<Domain.Entities.DayOfWeek> DaysOfWeek { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
