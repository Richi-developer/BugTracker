using BugTracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BugTracker.Database
{
    public class DatabaseContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Filename=Database.db");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bug>().HasKey(b => b.Id);
            modelBuilder.Entity<Bug>().Property(x => x.Id)
                .IsRequired().ValueGeneratedOnAdd();
        }

        public DbSet<Bug> Bugs { get; set; }

    }
}
