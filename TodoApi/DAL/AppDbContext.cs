using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User Model
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });

        }
    }
}
