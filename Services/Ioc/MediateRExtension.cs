using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MediatR;

namespace Services.Ioc
{
    public static class MediateRExtension
    {
        public static void ConfigureMediateRService(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddServerLocalization();
        }
    }
}
