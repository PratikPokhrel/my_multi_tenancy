using Core.Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Ioc
{
    internal static class ConfigureCommonServices
    {
        public static void ConfigureCommonService(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddTransient<ITenantProvider, TenantProvider>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<ITenantService, TenantService>();
            
        }
    }
}
