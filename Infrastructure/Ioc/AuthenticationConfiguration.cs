using Core.EF.IdentityModels;
using Microsoft.Extensions.DependencyInjection;
using Core.EF.Data.Context.Default;
using Microsoft.AspNetCore.Identity;
using Core.EF.Managers;
using Core.EF.Data.Configuration.Pg;

namespace Infrastructure.Ioc
{
    public static class AuthenticationConfiguration
    {
        public static void IdentityConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IRoleValidator<ApplicationRole>, MyRoleValidator>();
            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<ApplicationRole>()
                    .AddRoleValidator<MyRoleValidator>()
                    .AddEntityFrameworkStores<DefaultContext>();

            services.Configure<IdentityOptions>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
        }
    }
}
