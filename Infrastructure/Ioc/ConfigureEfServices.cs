using Core.EF.Data.Configuration;
using Core.EF.Data.Configuration.Management;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Ioc
{
    //public static class ConfigureEf
    //{
    //    public static IServiceCollection ConfigureEfServices(this IServiceCollection services, IConfiguration configuration)
    //    {
    //        EntityFrameworkConfiguration.ConfigureService(services, configuration);

    //        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    //        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    //        services.AddTransient<IDataBaseManager, DataBaseManager>();
    //        services.AddTransient<IContextFactory, ContextFactory>();
    //        services.AddTransient<IUnitOfWork, UnitOfWork>();
    //        services.AddTransient<IDefaultUnitOfWork, DefaultUow>();
    //        return services;
    //    }
    //}
}
