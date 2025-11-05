using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Context
{
    public class UserServiceDbContext : DbContext
    {
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<NoviceDriver> NoviceDrivers { get; set; }
        public DbSet<SavedLocation> SavedLocations { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<PersonalSchedule> PersonalSchedules { get; set; }
        public DbSet<ApplicationTracking> InstructorApplications { get; set; }
        public DbSet<InstructorApplication> InstructorDocuments { get; set; }
        //public DbSet<LicenseCategory> LicenseCategories { get; set; }
        public DbSet<Policy> Policies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
            //ConfigureLazyLoading(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
