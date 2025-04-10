

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BugTracker.Data;
using BugTracker.Core.AutoMapper;
using BugTracker.Core.Services;

namespace BugTracker.Core
{
    public static class Module
    {

        public static void AddBugTrackerCoreServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddBugTrackerDataServices();
            serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(BugProfile)));
            serviceCollection.AddScoped<BugsService>();
        }

    }
}
