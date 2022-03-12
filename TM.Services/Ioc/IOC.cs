using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TM.Services.Issues;
using TM.Services.Projects;

namespace TM.Services.Ioc
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureTMServices(this IServiceCollection services)
        {
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IIssueService, IssueService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
