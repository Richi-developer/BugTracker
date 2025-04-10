using Microsoft.Extensions.DependencyInjection;
using System;
using BugTracker.Data.Database;
using BugTracker.Data.Model;

namespace BugTracker.Data
{
    public static class Module
    {
        public static void AddBugTrackerDataServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEntityFrameworkSqlite().AddDbContext<DatabaseContext>();
        }


        public static void PopulateSampleData()
        {
            using var db = new DatabaseContext();
            {
                if (db.Database.EnsureCreated())
                {
                    db.Bugs.Add(new Bug() { Name = "Bug1", Description = "Не работает запись в чакру", Author = "Вася" });
                    db.Bugs.Add(new Bug() { Name = "Bug2", Description = "Не работает чтение из конденсатора потока", Author = "Петя", Status = BugStatuses.Fixed });
                    db.Bugs.Add(new Bug() { Name = "Bug3", Description = "Слишком мощные девиации", Author = "Лола", Status = BugStatuses.Closed });
                    db.SaveChanges();
                }
            }


        }
    }
}
