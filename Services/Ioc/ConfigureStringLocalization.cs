using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Ioc
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServerLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(ServerLocalizer<>));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
