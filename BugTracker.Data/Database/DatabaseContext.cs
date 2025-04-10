using BugTracker.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data.Database
{
    public class DatabaseContext:DbContext
    {
        private static bool _created = false;

        public DatabaseContext()
        {
            if(_created)
                return;
            Database.EnsureCreated();
            _created = true;

        }

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
