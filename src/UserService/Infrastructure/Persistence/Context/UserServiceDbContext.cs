using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Context
{
    public class UserServiceDbContext : DbContext
    {
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
        {
        }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }        
        public DbSet<SavedLocation> SavedLocations { get; set; }
        public DbSet<NoviceDriver> NoviceDrivers { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<PersonalSchedule> PersonalSchedules { get; set; }
        public DbSet<ApplicationTracking> InstructorApplications { get; set; }
        public DbSet<InstructorApplication> InstructorDocuments { get; set; }
        public DbSet<SystemDocument> SystemDocuments { get; set; }
        public DbSet<Policy> Policies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
