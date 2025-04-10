

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BugTracker.Data;
using BugTracker.Core.AutoMapper;
using BugTracker.Core.Services;
using Microsoft.Extensions.Configuration;

namespace BugTracker.Core
{
    public static class Module
    {

        public static void AddBugTrackerCoreServices(this IServiceCollection serviceCollection)
        {
            var thisAssembly = typeof(Module).Assembly;
            serviceCollection.AddBugTrackerDataServices();
            serviceCollection.AddAutoMapper(thisAssembly);
            serviceCollection.AddScoped<BugsService>();
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(thisAssembly));

        }

    }
}
