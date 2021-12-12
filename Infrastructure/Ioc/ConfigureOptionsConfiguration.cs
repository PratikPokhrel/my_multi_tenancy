using Core;
using Core.Constants;
using Core.EF.Data;
using Core.EF.Data.Configuration;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Ioc
{
    public static class ConfigureOptionsConfiguration
    {
        public static void ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<ConnectionSettings>(configuration.GetSection(DefaultConstants.ConnectionStrings));
            services.Configure<AppSettings>(configuration.GetSection(DefaultConstants.AppSettings));
            services.Configure<AuthenticationSettings>(configuration.GetSection(DefaultConstants.AuthenticationSettings));

            // Explicitly register the settings object so IOptions not required (optional)
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ConnectionSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AuthenticationSettings>>().Value);

            // Register as an IValidatable
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<ConnectionSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<AppSettings>>().Value);
        }
    }
}
