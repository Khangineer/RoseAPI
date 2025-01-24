using Microsoft.EntityFrameworkCore;

namespace RoseAPI.Entities
{
    public class RoseDBContext : DbContext
    {
        public RoseDBContext() { }
        public RoseDBContext(DbContextOptions<RoseDBContext> options) : base(options) { }

        public DbSet<Task> Task { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            base.OnModelCreating(modelBuilder);

        }
    }
}
