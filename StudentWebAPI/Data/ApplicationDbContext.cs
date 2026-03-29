using Microsoft.EntityFrameworkCore;
using StudentWebAPI.Model;

namespace StudentWebAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
            .HasIndex(x => x.Email)
            .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(x=>x.PhoneNumber)
                .IsUnique();
        }
    }
}