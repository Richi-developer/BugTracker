
using BugTracker.Database;
using System.Reflection;
using BugTracker.Model;

namespace BugTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });
            builder.Services.AddEntityFrameworkSqlite().AddDbContext<DatabaseContext>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


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
            
            app.Run();
        }
    }
}
