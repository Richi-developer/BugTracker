using BugTracker.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddEntityFrameworkSqlite().AddDbContext<DatabaseContext>(builder =>
            {
                builder.UseSqlite($"Filename=DatabaseTests_{Guid.NewGuid()}.db");
            });
            
            BugTracker.Core.Module.AddBugTrackerCoreServices(serviceCollection);
        }

    }
}
