using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<NoviceDriver> NoviceDrivers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<ApplicationTracking> InstructorApplications { get; set; }
        public DbSet<InstructorApplication> InstructorDocuments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            //ConfigureLazyLoading(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
