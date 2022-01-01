using Core.EF.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LazyCache;

namespace Infrastructure.Ioc
{
    public static class StartUp
    {
        public static void ConfigureAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureService(configuration);
            services.ConfigureEFService(configuration);
            services.ConfigureCommonService(configuration);
            services.ConfigureAuthConfiguration();
            services.AddLazyCache();
        }
    }
}
