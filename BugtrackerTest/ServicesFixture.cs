using BugTracker.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace BugTrackerTest
{
    public class ServicesFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<DatabaseContext>(builder =>
            {
                builder.UseSqlite($"Filename=DatabaseTests_{Guid.NewGuid()}.db");
            }, ServiceLifetime.Transient);

            BugTracker.Core.Module.AddBugTrackerCoreServices(services);
        }

        protected override IEnumerable<TestAppSettings> GetTestAppSettings() => new List<TestAppSettings>();

        protected override ValueTask DisposeAsyncCore() => new();
    }
}
