using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Plan> Plans => Set<Plan>();
        public DbSet<Week> Weeks => Set<Week>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Plans)
                .WithOne(p => p.User!)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Weeks)
                .WithOne(w => w.Plan!)
                .HasForeignKey(w => w.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Week>()
                .HasMany(w => w.Tasks)
                .WithOne(t => t.Week!)
                .HasForeignKey(t => t.WeekId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
