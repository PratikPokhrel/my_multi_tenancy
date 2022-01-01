using Core.EF.Data.Configuration.Management;
using Core.EF.Managers;
using Core.Infrastructure.DataAccess;
using Core.Security.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.EF.Data.Configuration
{
    /// <summary>
    /// IOC contaner start-up configuration
    /// </summary>
    public static class IocContainerConfiguration
    {
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureEFService(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureService(configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IContextFactory, ContextFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDefaultUnitOfWork, DefaultUow>();
            services.AddTransient<IPager, Pager>();



            //Security
            services.AddTransient<IApplictionUserManager,ApplicationUserManager>();
            services.AddTransient<IRoleManager, RoleManager>();
            services.AddTransient<IRoleClaimManager,RoleClaimManager>();
        }
    }
}