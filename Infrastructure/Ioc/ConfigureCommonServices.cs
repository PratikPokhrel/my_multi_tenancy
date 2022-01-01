using Core.ClientInfo;
using Core.Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using Services.Ioc;
using Services.Services.Identity;
using Services.Services.Branches;
using Core.Infrastructure.DataAccess;
using Core.EF.Managers;
using Infrastructure.Services;
using Core.Infrastructure;

namespace Infrastructure.Ioc
{
    internal static class ConfigureCommonServices
    {
        public static void ConfigureCommonService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ITenantProvider, TenantProvider>();
            services.AddTransient<ITenantSource, FileTenantSource>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IBranchService, BranchService>();
            services.AddTransient<IBranchUserService,BranchUserService>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IResponseLogService, ResponseLogService>();
            services.AddTransient<IClientInfoProvider, ClientInfoProvider>();
            services.AddTransient<IPager, Pager>();

            //Identity Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            MediateRExtension.ConfigureMediateRService(services);

            //
            services.AddTransient<IExcelService, ExcelService>();
        }
    }
}
