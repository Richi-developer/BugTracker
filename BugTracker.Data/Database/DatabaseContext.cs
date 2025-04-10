using BugTracker.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace BugTracker.Data.Database
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if(!optionsBuilder.IsConfigured)
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
